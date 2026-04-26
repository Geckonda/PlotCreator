using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Configurations
{
    public class EventConfiguration : EntityBaseConfiguration<Event>
    {
        protected override void ConfigureWorldRelation(EntityTypeBuilder<Event> builder)
        {
            builder.HasOne(e => e.World)
                .WithMany(w => w.Events)
                .HasForeignKey(e => e.WorldId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
