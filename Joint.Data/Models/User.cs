namespace Joint.Data.Models;

public class User
{
    public int Id { get; set; }
    
    public string? Email { get; set; }
    
    public string Login { get; set; }
    
    public string Password { get; set; }
    
    public string? Phone { get; set; }
    
    public List<Camera>? Cameras { get; set; }
    
}