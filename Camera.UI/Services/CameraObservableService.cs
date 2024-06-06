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
using DynamicData;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Ocl;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Joint.Data.Models;
using LibVLCSharp.Avalonia;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Flann;
using Emgu.CV.Structure;
using Emgu.Util;
using Joint.Data.Constants;
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
    private bool _isOpened = false;
    private DateTime? _lastDetectedMotion = null;
    private DispatcherTimer timer = new DispatcherTimer();
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

        if (capture != null)
        {
            capture.Stop();
            capture.Dispose();
            timer.Stop();
        }
        
        
        if (camera.Connection == CameraConnection.Ethernet)
        {
            capture = new VideoCapture(camera.ConnectionData);
        }
        else
        {
            capture = new VideoCapture(int.Parse(_camera.ConnectionData));
        }
        
        
        _isOpened = capture.IsOpened;
        if (capture.IsOpened)
        {
           timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(33);
            timer.Tick += GrabImage;
            timer.Start();
        }

        return _isOpened;
    }

    private async  void GrabImage(object sender, EventArgs e)
    {
        if (capture == null || capture.Ptr == nint.Zero)
        {
            return;
        }
        
        CascadeClassifier faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");
        Mat frame = capture.QueryFrame();
        if (frame != null && _prevFrame != null)
        {
            if (ReactToMotion)
            {
                // Преобразование кадра в чёрно-белый (градаций серого)
                Mat grayFrame = new Mat();
                CvInvoke.CvtColor(frame, grayFrame, ColorConversion.Bgr2Gray);

                // Детекция лиц на кадре
                Rectangle[] faces = faceCascade.DetectMultiScale(grayFrame, 1.1, 3, Size.Empty);

                // Отрисовка прямоугольников вокруг обнаруженных лиц (людей)
                foreach (Rectangle face in faces)
                {
                    CvInvoke.Rectangle(frame, face, new MCvScalar(0, 255, 0), 2);
                    if (faces.IndexOf(face) == 1 && (_lastDetectedMotion == null || DateTime.Now - _lastDetectedMotion > TimeSpan.FromMinutes(5) ))
                    {
                        _lastDetectedMotion = DateTime.Now;
                        await SendNotificationToServer(frame.ToBitmap());
                    }
                }
            }

            ImageGrab(this, new GrabImageEventArgs(frame.ToBitmap().ConvertToAvaloniaBitmap())); 
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
        
        //test
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