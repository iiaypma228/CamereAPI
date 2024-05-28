using Camera.DAL.Models;
using Joint.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Camera.DAL;

public class ServerContext : DbContext
{
    public ServerContext(DbContextOptions<ServerContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CameraConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationToSendConfiguration());
        modelBuilder.ApplyConfiguration(new CameraNotifiesConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
    
    public DbSet<Joint.Data.Models.Camera> Cameras { get; set; }
    
    public DbSet<Notification> Notifications { get; set; }
    
    public DbSet<NotifyToSend> NotifyToSends { get; set; }
    
    public DbSet<CameraNotifies> CameraNotifies { get; set; }
    
}