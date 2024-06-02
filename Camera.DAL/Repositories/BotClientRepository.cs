using Camera.DAL.Interfaces.Repositories;
using Joint.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera.DAL.Repositories
{
    public class BotClientRepository : Repository<BotClient>, IBotClientRepository
    {
        public BotClientRepository(ServerContext context) : base(context)
        {
        }
    }
}
