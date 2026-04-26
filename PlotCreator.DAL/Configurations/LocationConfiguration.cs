using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Configurations
{
    public class LocationConfiguration : EntityBaseConfiguration<Location>
    {
        protected override void ConfigureWorldRelation(EntityTypeBuilder<Location> builder)
        {
            builder.HasOne(e => e.World)
                .WithMany(w => w.Locations)
                .HasForeignKey(e => e.WorldId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
