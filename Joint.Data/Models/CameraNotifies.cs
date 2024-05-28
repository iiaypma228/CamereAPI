namespace Joint.Data.Models;

public class CameraNotifies
{
    public int CameraId { get; set; }
    public Camera? Camera { get; set; }
    
    public int NotificationId { get; set; }
    public Notification? Notification { get; set; }
    
}