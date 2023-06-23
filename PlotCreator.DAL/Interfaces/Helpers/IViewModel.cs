using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.DAL.Interfaces.Helpers
{
    public interface IViewModel<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityParam">Необязательный парметр. На случай, если необходим параметр для фильтра.</param>
        /// <returns>Пустая модель, заполненная данными из справочников.</returns>
        Task<T> GetReferenceData(int? entityParam = 0);
    }
}
