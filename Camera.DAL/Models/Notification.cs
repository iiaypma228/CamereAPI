using Joint.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camera.DAL.Models;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable(name: nameof(Tables.Notification)).HasKey(o => o.Id);

        builder.HasOne<User>().WithMany().HasForeignKey(i => i.UserId);

        builder.Ignore(i => i.User);
    }
}