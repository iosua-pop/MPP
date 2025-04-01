using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentieTurism.Utils
{
    public interface Observable<E> where E : Event
    {
        void AddObserver(Observer<E> e);
        void RemoveObserver(Observer<E> e);
        void NotifyObservers(E t);
    }
}
