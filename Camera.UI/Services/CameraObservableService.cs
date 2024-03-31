using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Avalonia.Media.Imaging;
using Avalonia.Remote.Protocol.Viewport;
using Avalonia.Threading;
using Camera.UI.Extensions;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using LibVLCSharp.Avalonia;
using LibVLCSharp.Shared;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace Camera.UI.Services;

public interface ICameraObservableService
{
    bool IsOpened { get; }
    bool ReactToMotion { get; set; }
    event EventHandler ImageGrab;
    bool StartObservable(int camIndex);
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
    private VideoCapture capture;
    private Mat _prevFrame;
    private double previousArea ;
    private BackgroundSubtractorMOG2 bgSubtractor = new BackgroundSubtractorMOG2();
    private bool motionDetected = false;
    private DateTime motionStart = DateTime.Now;
    private bool _isOpened = false;
    
    
    public bool ReactToMotion { get; set; } = true;
    public bool IsOpened
    {
        get => _isOpened;
    }
    public event EventHandler ImageGrab;

    public bool StartObservable(int camIndex = 0)
    {
        capture = new VideoCapture(camIndex);
        
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

    private void GrabImage(object sender, EventArgs e)
    {
        Mat frame = capture.QueryFrame();
        if (frame != null && _prevFrame != null)
        {
            if (ReactToMotion)
            {
                Mat foregroundMask = new Mat();
                bgSubtractor.Apply(frame, foregroundMask);
                CvInvoke.Threshold(foregroundMask, foregroundMask, 190, 255, ThresholdType.Binary);

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
                            if (motionDuration.TotalSeconds > 5)
                            {
                                motionStart = DateTime.Now;
                                //TODO SEND MOTION DETECTED
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
    
    
}