using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Remote.Protocol.Viewport;
using Avalonia.Threading;
using Camera.UI.Extensions;
using DirectShowLib;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Ocl;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Joint.Data.Models;
using LibVLCSharp.Avalonia;
using LibVLCSharp.Shared;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Device = Emgu.CV.Dai.Device;

namespace Camera.UI.Services;

public interface ICameraObservableService
{
    bool IsOpened { get; }
    bool ReactToMotion { get; set; }
    event EventHandler ImageGrab;
    bool StartObservable(Joint.Data.Models.Camera camera);
}

// Создаем класс для данных, связанных с событием
public class GrabImageEventArgs : EventArgs
{
    public Bitmap Bitmap { get; }

    public GrabImageEventArgs(Bitmap bitmap)
    {
        this.Bitmap = bitmap;
    }
}

public class CameraObservableService : ICameraObservableService
{
    private readonly HttpClient _httpClient;
    private Joint.Data.Models.Camera _camera;
    private VideoCapture capture;
    private Mat _prevFrame;
    private double previousArea ;
    private BackgroundSubtractorMOG2 bgSubtractor = new BackgroundSubtractorMOG2();
    private bool motionDetected = false;
    private DateTime motionStart = DateTime.Now;
    private bool _isOpened = false;

    public CameraObservableService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public bool ReactToMotion { get; set; } = true;
    public bool IsOpened
    {
        get => _isOpened;
    }
    public event EventHandler ImageGrab;

    public bool StartObservable(Joint.Data.Models.Camera camera)
    {
        _camera = camera;
        capture = new VideoCapture(int.Parse(_camera.ConnectionData));
        
        _isOpened = capture.IsOpened;
        if (capture.IsOpened)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(33);
            timer.Tick += GrabImage;
            timer.Start();
        }

        return _isOpened;
    }

    private async  void GrabImage(object sender, EventArgs e)
    {
        Mat frame = capture.QueryFrame();
        if (frame != null && _prevFrame != null)
        {
            if (ReactToMotion)
            {
                Mat foregroundMask = new Mat();
                bgSubtractor.Apply(frame, foregroundMask);
                CvInvoke.Threshold(foregroundMask, foregroundMask, 200, 255, ThresholdType.Binary);

                // Ищем контуры для выделения объектов
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(foregroundMask, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

                // Обходим каждый контур и вычисляем его площадь
                for (int i = 0; i < contours.Size; i++)
                {
                    double area = CvInvoke.ContourArea(contours[i]);
                    if (area > 500) // Произвольный порог площади, который определяет, что это движущийся объект
                    {
                        // Движение обнаружено
                        if (!motionDetected)
                        {
                            motionStart = DateTime.Now;
                            motionDetected = true;
                        }
                        else
                        {
                            TimeSpan motionDuration = DateTime.Now - motionStart;
                            if (motionDuration.TotalSeconds > 6)
                            {
                                motionStart = DateTime.Now;
                                ReactToMotion = false;
                                //TODO SEND MOTION DETECTED
                                await SendNotificationToServer(frame.ToBitmap());
                            }
                        }
                    }
                }
            }

            var bitmap = frame.ToBitmap();
            ImageGrab(this, new GrabImageEventArgs(bitmap.ConvertToAvaloniaBitmap()));            
            //VideoFrame = bitmap.ConvertToAvaloniaBitmap();
        }
        _prevFrame = frame;
    }

    private async Task SendNotificationToServer(System.Drawing.Bitmap photo)
    {
        var notifyToSend = new NotifyToSend()
        {
            UserId = _camera.UserId,
            Camera = _camera,
            CameraId = _camera.Id,
            Date = DateTime.Now,
            Message = $"{DateTime.Now} виявлено рух!"
        };
        
        
        // Создаем MultipartFormDataContent для отправки файлов и формы
        var formData = new MultipartFormDataContent();

        // Добавляем объект notify в форму
        formData.Add(new StringContent(notifyToSend.Date.ToString()), "Date");
        formData.Add(new StringContent(notifyToSend.CameraId.ToString()), "CameraId");
        formData.Add(new StringContent(notifyToSend.Message), "Message");
        
        
        byte[] imageBytes;
        using (MemoryStream ms = new MemoryStream())
        {
            photo.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            imageBytes = ms.ToArray();
        }
        
        // Добавляем изображение в форму
        var imageContent = new ByteArrayContent(imageBytes);
        imageContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("image/jpeg");
        formData.Add(imageContent, "file", "image.jpg");

        // Отправляем POST запрос на метод контроллера
        var response = await _httpClient.PostAsync("api/Notification/send", formData);

    }
}