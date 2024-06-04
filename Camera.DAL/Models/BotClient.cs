using Joint.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera.DAL.Models
{
    public class BotClientConfiguration : IEntityTypeConfiguration<BotClient>
    {
        public void Configure(EntityTypeBuilder<BotClient> builder)
        {
            builder.ToTable(name: nameof(Tables.BotClient)).HasKey(o => o.Id);
            builder.HasOne<User>().WithMany().HasForeignKey(i => i.UserId);
            builder.Ignore(i => i.User);
        }
    }
}
