using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Enum
{
    public enum StatusCode
    {
        Ok = 200,//Все хорошо
        Forbidden = 403,//Доступ запрещён
        NotFound = 404,//Страница не найдена
        Conflict = 409,//Конфликт (дубликат или нарушение инварианта)
        InternalServerError = 500//Внутренняя ошибка сервера
    }
}
