using System.Threading.Tasks;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Response.Implementations;
using PlotCreator.Domain.ViewModels.Account;

namespace PlotCreator.Service.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<AuthResultDto>> Register(RegisterViewModel model);
        Task<BaseResponse<AuthResultDto>> Login(LoginViewModel model);
        Task<User?> GetUser(int id);
        Task<AuthUserDto?> GetCurrentAsync(int userId);
    }
}
