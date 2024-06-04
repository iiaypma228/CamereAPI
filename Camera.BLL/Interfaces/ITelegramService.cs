using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera.BLL.Interfaces
{
    public interface ITelegramService : INotificationService, ICRUDService<Joint.Data.Models.BotClient>
    {
        void Init();
    }
}
