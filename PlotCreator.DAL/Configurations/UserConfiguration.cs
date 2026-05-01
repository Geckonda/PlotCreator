using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed the default user (Id=1) used by ICurrentUserService until auth is wired.
            // Password hash: SHA256("default") via HashPasswordHelper.
            builder.HasData(new User
            {
                Id = 1,
                RoleId = 3,
                Nickname = "Default",
                Login = "default",
                Email = "default@local",
                Password = "37a8eec1ce19687d132fe29051dca629d164e2c4958ba141d5f4133a33f0688f"
            });
        }
    }
}
