using PlotCreator.Service.Interfaces;

namespace PlotCreator.Service.Implementations
{
    /// Returns the seeded default user (Id=1) until auth is wired.
    public sealed class DefaultUserService : ICurrentUserService
    {
        public int GetUserId() => 1;
    }
}
