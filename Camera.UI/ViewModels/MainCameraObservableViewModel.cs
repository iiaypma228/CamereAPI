using System;
using Camera.UI.Services;
using LibVLCSharp.Avalonia;
using LibVLCSharp.Shared;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Camera.UI.ViewModels;

public class MainCameraObservableViewModel : RoutableViewModelBase
{
    private ICameraObservableService _service;

    public MediaPlayer MediaPlayer { get; }

    private LibVLC _libVlc;
    private VideoView _videoView;
    public VideoView VideoView
    {
        get => _videoView;
        set
        {
            _videoView = value;
            
            //_videoView.P
            //"dshow://"
            //FromType.FromLocation
            //this.RaiseAndSetIfChanged()
            // _service.StartObservable(_videoView);
        }
    }

    public MainCameraObservableViewModel(ICameraObservableService cameraObservableService, IScreen screen, RoutingState routingState) : base(screen, routingState)
    {
        _libVlc = new LibVLC();
        MediaPlayer = new MediaPlayer(_libVlc);
        _service = cameraObservableService;

    }

    public void PlayVideo()
    {
        MediaPlayer.Media = new  Media(_libVlc, "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4", FromType.FromLocation);
        VideoView.MediaPlayer.Play();
    }
}