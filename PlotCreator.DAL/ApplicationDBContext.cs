using Microsoft.EntityFrameworkCore;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<World> Worlds => Set<World>();
        public DbSet<EntityType> EntityTypes => Set<EntityType>();
        public DbSet<WorldEntity> Entities => Set<WorldEntity>();
        public DbSet<Relation> Relations => Set<Relation>();

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
