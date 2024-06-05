using Joint.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camera.DAL.Models;

public class CameraNotifiesConfiguration : IEntityTypeConfiguration<Joint.Data.Models.CameraNotifies>
{
    public void Configure(EntityTypeBuilder<Joint.Data.Models.CameraNotifies> builder)
    {
        builder.ToTable(name: nameof(Tables.CameraNotifies)).HasKey(o => new {o.CameraId, o.NotificationId});

        /*builder.HasOne<Joint.Data.Models.Camera>().WithMany().HasForeignKey(i => i.CameraId);
        builder.HasIndex(o => o.CameraId).IsUnique(false);
        builder.HasOne<Joint.Data.Models.Camera>().WithMany().HasForeignKey(i => i.NotificationId);
        builder.HasIndex(o => o.NotificationId).IsUnique(false);*/
        //builder.HasOne<Joint.Data.Models.Camera>().WithMany().HasForeignKey(i => i.CameraId);

        //builder.Ignore(i => i.Camera);
        builder.Ignore(i => i.Camera);
        builder.Ignore(i => i.Notification);
    }
}