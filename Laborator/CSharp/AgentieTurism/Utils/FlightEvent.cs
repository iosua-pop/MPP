using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentieTurism.Utils
{
    public class FlightEvent : Event
    {
        public enum EventType { FLIGHTS_UPDATED }

        public EventType Type { get; }

        public FlightEvent(EventType type)
        {
            Type = type;
        }
    }
}
