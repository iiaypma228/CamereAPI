namespace Joint.Data.Models;

public class NotifyToSend
{
    public int Id { get; set; }
    
    public bool IsSent { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }
    public int CameraId { get; set; }
    public Camera? Camera { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }
    public bool IsNeedToNotify { get; set; } = true;
    public string? PathToFile { get; set; }
    public string? SendAddress { get; set; } // Email, Phone
}