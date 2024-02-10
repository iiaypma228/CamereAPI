using Joint.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Camera.DAL.Models;

public class UserConfiguration : IEntityTypeConfiguration<User>
{

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(name: nameof(Tables.User)).HasKey(o => o.Id);
        builder.Ignore(i => i.Cameras);
    }
}