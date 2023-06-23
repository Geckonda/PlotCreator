using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlotCreator.Domain.Entity;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces.Helpers
{
    /// <summary>
    /// Интерфейс-посредник. Позволяет применять CRUD-операции над связями сущностей, относящиеся к сущности <see cref="Group"/> (М:М)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGroupMediator<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>Группы, привязанные к сущности T.</returns>
        Task<IQueryable<T>> GetEntityGroups(int entityId);
        /// <summary>
        /// Возвращает все группы книги по <c>bookId</c>.
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns>Группы книги</returns>
        Task<IQueryable<T>> GetAllEntityGroupsByBookId(int bookId);
        Task AddGroupsToEntity(IEnumerable<T> groups);
        Task DeleteGroupsFromEntity(IEnumerable<T> groups);
        /// <summary>
        /// Добавляет связи <see cref="Group"/> и T, посступающие в параметре, остальные удаляет.
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="entityId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        Task EditGroupsEntityRelation(IEnumerable<T> groups, int entityId, int bookId);
    }
}
