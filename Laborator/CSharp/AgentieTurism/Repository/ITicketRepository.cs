using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentieTurism.Models;

namespace AgentieTurism.Repository
{
    public interface ITicketRepository : IRepository<Ticket, int>
    {
        List<Ticket> FindByFlightId(int flightId);
    }
}
