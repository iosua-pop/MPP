using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentieTurism.Models;

namespace AgentieTurism.Repository
{
    public interface IRepository<T, ID> where T : IIdentifiable<ID>
    {
        void Add(T entity);
        void Update(ID id, T entity);
        void Delete(ID id);
        T FindOne(ID id);
        List<T> FindAll();
    }
}