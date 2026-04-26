using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Configurations
{
    public class FactionConfiguration : EntityBaseConfiguration<Faction>
    {
        protected override void ConfigureWorldRelation(EntityTypeBuilder<Faction> builder)
        {
            builder.HasOne(e => e.World)
                .WithMany(w => w.Factions)
                .HasForeignKey(e => e.WorldId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
