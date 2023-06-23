using PlotCreator.Domain.Enum;
using PlotCreator.Domain.Response.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Response.Implementations
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string Description { get; set; }
        public StatusCode StatusCode { get; set; }
        public T Data { get; set; }
        public string ErrorForUser { get; set; }
    }
}
