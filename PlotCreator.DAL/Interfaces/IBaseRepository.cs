using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<bool> Add(T entity);
        Task<bool> Delete(T entity);
        Task<T> Update(T entity);
        IQueryable<T> GetAll();
    }
}
