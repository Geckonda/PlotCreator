using PlotCreator.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces.Helpers
{
    public interface IUserEntity<T>
    {
        Task<IQueryable<T>> GetAllByUserId(int userId);
        Task<int> GetUserId(int entityId);
        Task<User> GetUser(int entityId);
        /// <summary>
        /// Возвращает конкретную последнюю, добавленную пользователем сущность.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetLastUserEntityId(int userId);
    }
}
