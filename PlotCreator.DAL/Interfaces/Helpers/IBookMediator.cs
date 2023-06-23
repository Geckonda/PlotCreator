using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Interfaces.Helpers
{
    /// <summary>
    /// Интерфейс-посредник. Позволяет применять CRUD-операции над связями сущностей, относящиеся к сущности <see cref="Book"/>  (М:М).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBookMediator<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="entityIds">Id экземпляров сущности T.</param>
        /// <returns>Список сущностей T, связанных с сущностью <see cref="Book"/>.</returns>
        IQueryable<T> GetBookEntitiesRelations(int bookId, int[] entityIds);
        /// <summary>
        /// Соединяет экземпляры сущности T с сущностью <see cref="Book"/> в БД.
        /// </summary>
        /// <param name="entities">Экземпляры сущности Т, с которыми необходимо установить связь с сущностью <see cref="Book"/>.</param>
        /// <returns></returns>
        Task AddEntitiesToBook(IEnumerable<T> entities);
        /// <summary>
        /// Удаляет связь экземпляров сущности T и сущности <see cref="Book"/> в БД.
        /// </summary>
        /// <param name="entities">Экземпляры сущности Т, с которыми необходимо удалить связь с сущностью <see cref="Book"/>.</param>
        /// <returns></returns>
        Task DeleteEntitiesFromBook(IEnumerable<T> entities);
    }
}
