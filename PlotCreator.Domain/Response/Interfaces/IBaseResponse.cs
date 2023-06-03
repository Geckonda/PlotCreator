using PlotCreator.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Response.Interfaces
{
    public interface IBaseResponse<T>
    {
        T Data { get; set; }
        public StatusCode StatusCode { get; set; }

        public string Description { get; set; }
        public string ErrorForUser { get; set; }
    }
}
