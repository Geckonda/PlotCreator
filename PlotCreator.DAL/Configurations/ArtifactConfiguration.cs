using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Configurations
{
    public class ArtifactConfiguration : EntityBaseConfiguration<Artifact>
    {
        protected override void ConfigureWorldRelation(EntityTypeBuilder<Artifact> builder)
        {
            builder.HasOne(e => e.World)
                .WithMany(w => w.Artifacts)
                .HasForeignKey(e => e.WorldId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
