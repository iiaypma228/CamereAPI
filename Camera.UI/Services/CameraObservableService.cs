using System;
using System.Collections.Generic;
using System.Drawing;
using Avalonia.Media.Imaging;
using Avalonia.Remote.Protocol.Viewport;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using LibVLCSharp.Avalonia;
using LibVLCSharp.Shared;

namespace Camera.UI.Services;

public interface ICameraObservableService
{
    
    void StartObservable(VideoView view, MediaPlayer player);
}

public class CameraObservableService : ICameraObservableService
{
    private VideoView _videoView;
    private MediaPlayer _mediaPlayer;
    public VideoCapture Capture { get; private set; }
    private bool _motionDetected;
    
    public void StartObservable(VideoView videoView, MediaPlayer player)
    {
        _mediaPlayer = player;
        _videoView = videoView;
        Capture = new VideoCapture(0);

        Core.Initialize();

        // Start the camera
        Capture.ImageGrabbed += VideoCapture_ImageGrabbed;
        Capture.Start();
    }
    private void VideoCapture_ImageGrabbed(object sender, EventArgs e)
    {
        /*// Get the frame from the camera and pass it to LibVLC
        using (var frame = new Mat())
        {
            Capture.Retrieve(frame);
        
            // Convert the Emgu.CV image to System.Drawing.Bitmap
            var bitmap = new Bitmap(frame.Width, frame.Height, frame.,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb, frame.Bitmap.Data);
        
            var byteBuffer = ImageToByte(bitmap);
            _mediaPlayer = new MediaPlayer(new Media());
        }*/
    }

    /*private byte[] ImageToByte(System.Drawing.Bitmap image)
    {
        // Convert the image to a byte array
        using (var stream = new System.IO.MemoryStream())
        {
            image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            return stream.ToArray();
        }
    }*/
    /*private void ProcessFrame(object sender, EventArgs e)
    {
        Mat frame = new Mat();
        Capture.Retrieve(frame, 0);

        if (!frame.IsEmpty)
        {
            // Convert EmguCV Mat to LibVLCSharp Bitmap
            BitmapFormatConverter.ToBitmap(frame, out IntPtr bitmapPtr, out _);
            var bitmap = new Bitmap(bitmapPtr, frame.Cols, frame.Rows, frame.Cols * 3, PixelFormat.BGR24);

            // Update the VideoView
            _videoView.Bi = bitmap;

            // Perform motion detection
            if (MotionDetected(frame))
            {
                // Motion detected, you can perform actions here
                _motionDetected = true;
            }
            else
            {
                _motionDetected = false;
            }
        }
    }
    
    private bool MotionDetected(Mat frame)
    {
        // Convert the frame to grayscale for motion detection
        CvInvoke.CvtColor(frame, frame, ColorConversion.Bgr2Gray);

        // Apply GaussianBlur to reduce noise and improve motion detection
        CvInvoke.GaussianBlur(frame, frame, new Size(21, 21), 0);


        // Set a threshold to identify motion
        double minValues = 10, maxValues = 50;
        Point minLoc = new Point() , maxLoc = new Point();
        CvInvoke.MinMaxLoc(frame, ref minValues, ref maxValues, ref minLoc, ref maxLoc);

        // You may need to experiment with the threshold value
        double threshold = 50;

        return maxValues > threshold;
    }*/

    
}