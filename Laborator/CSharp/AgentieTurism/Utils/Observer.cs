using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentieTurism.Utils
{
    public interface Observer<E> where E : Event
    {
        void Update(E e);
    }
}
