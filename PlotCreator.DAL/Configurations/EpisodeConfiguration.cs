using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Configurations
{
    public class EpisodeConfiguration : EntityBaseConfiguration<Episode>
    {
        protected override void ConfigureWorldRelation(EntityTypeBuilder<Episode> builder)
        {
            builder.HasOne(e => e.World)
                .WithMany(w => w.Episodes)
                .HasForeignKey(e => e.WorldId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
