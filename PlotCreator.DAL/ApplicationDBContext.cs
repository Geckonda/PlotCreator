using Microsoft.EntityFrameworkCore;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<World> Worlds => Set<World>();
        public DbSet<Character> Characters => Set<Character>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Faction> Factions => Set<Faction>();
        public DbSet<Episode> Episodes => Set<Episode>();
        public DbSet<Artifact> Artifacts => Set<Artifact>();
        public DbSet<Lore> Lores => Set<Lore>();
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
