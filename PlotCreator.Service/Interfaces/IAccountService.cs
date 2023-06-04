using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Response.Implementations;
using PlotCreator.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Service.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);

        Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);
        Task<User> GetUser(int id);
    }
}
