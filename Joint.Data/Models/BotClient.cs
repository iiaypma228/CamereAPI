using Joint.Data.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joint.Data.Models
{
    public class BotClient
    {
        public int Id { get; set; }

        public long TelegramId { get; set; }

        public int? UserId { get; set; }

        public User? User { get; set; }

        public BotSteps Step { get; set; }

    }
}
