using Joint.Data.Constants;

namespace Joint.Data.Models;

public class Notification
{
    public int Id { get; set; }
    
    public int UserId { get; set; }

    public User? User { get; set; }
    
    public string Name { get; set; }
    
    public NotificationType NotificationType { get; set; }
    
    public string Data { get; set; } //например номер телефоне, почта, зависит от типа нотификации
    
}