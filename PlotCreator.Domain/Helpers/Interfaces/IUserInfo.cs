using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Helpers.Interfaces
{
    public interface IUserInfo
    {
        Task<int> GetUserId(int contentId);
    }
}
