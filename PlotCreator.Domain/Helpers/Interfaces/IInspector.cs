using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Helpers.Interfaces
{
    //Проверяет совпадает ли авторизированный userId  с userId из запроса.
    public interface IInspector<T>
    {
        Task<T> CheckByContentId(int contentId);

        T CheckByUserId(int userId);
    }
}
