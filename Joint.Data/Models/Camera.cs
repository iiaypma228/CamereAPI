using Joint.Data.Constants;

namespace Joint.Data.Models;

public class Camera
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public User? User { get; set; }
    
    public string Name { get; set; }
    
    public CameraConnection Connection { get; set; }
    
    public string ConnectionData { get; set; }

    public List<Notification> Notifications { get; set; }

}