using PlotCreator.Domain.Entity;

namespace PlotCreator.Service.Interfaces
{
    public interface IJwtTokenService
    {
        string IssueToken(User user);
        int ExpiresInDays { get; }
        string CookieName { get; }
    }
}
