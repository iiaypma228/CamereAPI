using Joint.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camera.DAL.Models;

public class NotificationToSendConfiguration : IEntityTypeConfiguration<NotifyToSend>
{
    public void Configure(EntityTypeBuilder<NotifyToSend> builder)
    {
        builder.ToTable(name: nameof(Tables.NotificationToSend)).HasKey(o => o.Id);

        builder.HasOne<User>().WithMany().HasForeignKey(i => i.UserId);
        builder.HasOne<Joint.Data.Models.Camera>().WithMany().HasForeignKey(i => i.CameraId);
        
        builder.Ignore(i => i.Camera);
        builder.Ignore(i => i.User);
    }
}