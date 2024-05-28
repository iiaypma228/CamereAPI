using Joint.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camera.DAL.Models;

public class CameraConfiguration : IEntityTypeConfiguration<Joint.Data.Models.Camera>
{

    public void Configure(EntityTypeBuilder<Joint.Data.Models.Camera> builder)
    {
        builder.ToTable(name: nameof(Tables.Camera)).HasKey(o => o.Id);

        builder.HasOne<User>().WithMany().HasForeignKey(i => i.UserId);
        builder.Ignore(i => i.User);
        builder.Ignore(i => i.Notifications);33333
    }
}