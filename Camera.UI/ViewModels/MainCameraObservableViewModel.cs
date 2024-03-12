using System;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Camera.UI.Extensions;
using Camera.UI.Services;
using Emgu.CV;
using Emgu.CV.Structure;
using LibVLCSharp.Avalonia;
using LibVLCSharp.Shared;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Camera.UI.ViewModels;

public class MainCameraObservableViewModel : RoutableViewModelBase
{
    private ICameraObservableService _service;
    private VideoCapture capture;
    [Reactive] public Bitmap VideoFrame { get; set; }
    public MainCameraObservableViewModel(ICameraObservableService cameraObservableService, IScreen screen, RoutingState routingState) : base(screen, routingState)
    {
        capture = new VideoCapture(0);
        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(33);
        timer.Tick += cameraFPS;
        timer.Start();
    }

    private void cameraFPS(object sender, EventArgs e)
    {
        Mat frame = capture.QueryFrame();
        
        if (frame != null)
        {
            var bitmap = frame.ToBitmap();
            
            VideoFrame = bitmap.ConvertToAvaloniaBitmap();
        }
    }
    
}