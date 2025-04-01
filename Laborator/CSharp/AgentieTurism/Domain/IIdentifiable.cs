using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentieTurism.Models
{
    public interface IIdentifiable<T>
    {
        T Id { get; set; }
    }
}
