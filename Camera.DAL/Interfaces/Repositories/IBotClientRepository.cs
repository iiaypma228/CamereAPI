using Joint.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera.DAL.Interfaces.Repositories
{
    public interface IBotClientRepository : IRepository<BotClient>, IDisposable { }
}
