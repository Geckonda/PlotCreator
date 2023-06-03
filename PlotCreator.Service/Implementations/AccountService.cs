using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;
using PlotCreator.Domain.Helpers;
using PlotCreator.Domain.Response.Implementations;
using PlotCreator.Domain.ViewModels.Account;
using PlotCreator.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IBaseRepository<User> userRepository,
            ILogger<AccountService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == model.Login);
                if(user == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Логин или пароль указаны неверно",
                    };
                }

                if(user.Password != HashPasswordHelper.HashPassword(model.Password))
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Логин или пароль указаны неверно",
                    };
                }
                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = StatusCode.Ok,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Login]: {ex.Message}]");
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == model.Login || x.Email == model.Email);
                if(user != null)
                {
                    if (user.Login == model.Login)
                    {
                        return new BaseResponse<ClaimsIdentity>()
                        {
                            Description = "Пользователь с таким логином уже зарегистрирован",
                        };
                    }
                    if (user.Email == model.Email)
                    {
                        return new BaseResponse<ClaimsIdentity>()
                        {
                            Description = "Пользователь с такой электронной почтой уже зарегистрирован",
                        };
                    }
                }
                user = new User()
                {
                    roleId = Convert.ToInt32(UserRole.User),
                    Nickname = model.Nickname,
                    Login = model.Login,
                    Email = model.Email,
                    Password = HashPasswordHelper.HashPassword(model.Password),
                };
                await _userRepository.Add(user);
                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    Description = "Пользователь зарегистрирован",
                    StatusCode = StatusCode.Ok
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"[Register]: {ex.Message}");
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }


        private ClaimsIdentity Authenticate(User user)
        {
            var role = CheckUserRole(user.roleId);
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login!),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
                new Claim("userId", user.Id.ToString() ),
                new Claim("login", user.Login!.ToString() ),
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
        
        private string CheckUserRole(int roleId)
        {
            switch(roleId)
            {
                case 1:
                    return "Admin";
                case 2:
                    return "Moderator";
                default:
                    return "User";
            }
        }
    }
}
