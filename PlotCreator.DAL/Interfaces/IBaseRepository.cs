using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task Add(T entity);
        Task Delete(T entity);
        Task<T> Update(T entity);
        IQueryable<T> GetAll();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentId">Id таблицы на уровень выше</param>
        /// <returns></returns>
        IQueryable<T> GetAll(int parentId);
    }
}
