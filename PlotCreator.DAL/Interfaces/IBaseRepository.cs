using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces
{
    /// <summary>
    /// Интерфейс, предоставляющий CRUD-операции с моделями репозиториев БД. 
    /// </summary>
    /// <typeparam name="T">Параметром может являтья любая модель БД.</typeparam>
    public interface IBaseRepository<T>
    {
        Task Add(T entity);
        Task Delete(T entity);
        Task<T> Update(T entity);
        T GetOne(int id);
        IQueryable<T> GetAll();
    }
}
