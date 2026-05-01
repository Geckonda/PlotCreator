using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlotCreator.DAL.Interfaces;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;
using PlotCreator.Domain.Helpers;
using PlotCreator.Domain.Response.Implementations;
using PlotCreator.Domain.ViewModels.Account;
using PlotCreator.Service.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PlotCreator.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IJwtTokenService _jwt;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IBaseRepository<User> userRepository,
            IJwtTokenService jwt,
            ILogger<AccountService> logger)
        {
            _userRepository = userRepository;
            _jwt = jwt;
            _logger = logger;
        }

        public async Task<User?> GetUser(int id)
        {
            try
            {
                return await Task.FromResult(_userRepository.GetOne(id));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<AuthUserDto?> GetCurrentAsync(int userId)
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) return null;
            return ToDto(user);
        }

        public async Task<BaseResponse<AuthResultDto>> Login(LoginViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Login == model.Login);
                if (user == null)
                    return Fail("Логин или пароль указаны неверно");

                if (user.Password != HashPasswordHelper.HashPassword(model.Password))
                    return Fail("Логин или пароль указаны неверно");

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Login]: {Msg}", ex.Message);
                return new BaseResponse<AuthResultDto>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<AuthResultDto>> Register(RegisterViewModel model)
        {
            try
            {
                var existing = await _userRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Login == model.Login || x.Email == model.Email);
                if (existing != null)
                {
                    if (existing.Login == model.Login)
                        return Fail("Пользователь с таким логином уже зарегистрирован");
                    if (existing.Email == model.Email)
                        return Fail("Пользователь с такой электронной почтой уже зарегистрирован");
                }

                var user = new User
                {
                    RoleId = (int)UserRole.User,
                    Nickname = model.Nickname,
                    Login = model.Login,
                    Email = model.Email,
                    Password = HashPasswordHelper.HashPassword(model.Password),
                };
                await _userRepository.Add(user);

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Register]: {Msg}", ex.Message);
                return new BaseResponse<AuthResultDto>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private BaseResponse<AuthResultDto> Ok(User user) => new()
        {
            Data = new AuthResultDto
            {
                Token = _jwt.IssueToken(user),
                ExpiresInDays = _jwt.ExpiresInDays,
                User = ToDto(user)
            },
            StatusCode = StatusCode.Ok
        };

        private static BaseResponse<AuthResultDto> Fail(string desc) => new()
        {
            Description = desc,
            ErrorForUser = desc,
            StatusCode = StatusCode.NotFound
        };

        private static AuthUserDto ToDto(User user) => new()
        {
            Id = user.Id,
            Login = user.Login ?? string.Empty,
            Nickname = user.Nickname ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Role = user.RoleId switch
            {
                (int)UserRole.Admin => "Admin",
                (int)UserRole.Moderator => "Moderator",
                _ => "User"
            }
        };
    }
}
