namespace PlotCreator.Domain.Contracts
{
    public sealed class AuthUserDto
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
    }

    public sealed class AuthResultDto
    {
        public string Token { get; set; } = string.Empty;
        public int ExpiresInDays { get; set; }
        public AuthUserDto User { get; set; } = new();
    }
}
