using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotCreator.Domain.Contracts;
using PlotCreator.Domain.Response.Implementations;
using PlotCreator.Domain.ViewModels.Account;
using PlotCreator.Service.Interfaces;
using StatusCodeEnum = PlotCreator.Domain.Enum.StatusCode;

namespace PlotCreator_API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _account;
        private readonly IJwtTokenService _jwt;
        private readonly ICurrentUserService _currentUser;

        public AccountController(
            IAccountService account,
            IJwtTokenService jwt,
            ICurrentUserService currentUser)
        {
            _account = account;
            _jwt = jwt;
            _currentUser = currentUser;
        }

        [HttpPost("api/auth/register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var result = await _account.Register(model);
            return Map(result, setCookie: true);
        }

        [HttpPost("api/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var result = await _account.Login(model);
            return Map(result, setCookie: true);
        }

        [HttpPost("api/auth/logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(_jwt.CookieName, BuildCookieOptions(expired: true));
            return Ok(new { ok = true });
        }

        [Authorize]
        [HttpGet("api/auth/me")]
        public async Task<IActionResult> Me()
        {
            var id = _currentUser.GetUserId();
            if (id <= 0) return Unauthorized();
            var dto = await _account.GetCurrentAsync(id);
            if (dto is null) return Unauthorized();
            return Ok(dto);
        }

        private IActionResult Map(BaseResponse<AuthResultDto> r, bool setCookie)
        {
            switch (r.StatusCode)
            {
                case StatusCodeEnum.Ok when r.Data is not null:
                    if (setCookie)
                        Response.Cookies.Append(_jwt.CookieName, r.Data.Token, BuildCookieOptions());
                    // Token is already in the cookie; expose user info to the SPA.
                    return Ok(r.Data.User);
                case StatusCodeEnum.NotFound:
                    return BadRequest(new { description = r.Description, errorForUser = r.ErrorForUser });
                default:
                    return StatusCode(500, new { description = r.Description, errorForUser = r.ErrorForUser });
            }
        }

        private CookieOptions BuildCookieOptions(bool expired = false)
        {
            var opts = new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Path = "/",
                IsEssential = true,
            };
            if (expired)
                opts.Expires = DateTimeOffset.UnixEpoch;
            else
                opts.Expires = DateTimeOffset.UtcNow.AddDays(_jwt.ExpiresInDays);
            return opts;
        }
    }
}
