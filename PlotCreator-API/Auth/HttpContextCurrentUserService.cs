using PlotCreator.Service.Interfaces;
using System.Security.Claims;

namespace PlotCreator_API.Auth
{
    /// Reads the userId claim from the authenticated principal.
    /// Returns 0 if no user is signed in.
    public sealed class HttpContextCurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _accessor;

        public HttpContextCurrentUserService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public int GetUserId()
        {
            var user = _accessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated != true) return 0;

            var raw = user.FindFirst("userId")?.Value
                ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(raw, out var id) ? id : 0;
        }
    }
}
