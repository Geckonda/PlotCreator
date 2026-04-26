using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Configurations
{
    public class LoreConfiguration : EntityBaseConfiguration<Lore>
    {
        protected override void ConfigureWorldRelation(EntityTypeBuilder<Lore> builder)
        {
            builder.HasOne(e => e.World)
                .WithMany(w => w.Lores)
                .HasForeignKey(e => e.WorldId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
