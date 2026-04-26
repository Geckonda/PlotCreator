using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Configurations
{
    public class CharacterConfiguration : EntityBaseConfiguration<Character>
    {
        protected override void ConfigureWorldRelation(EntityTypeBuilder<Character> builder)
        {
            builder.HasOne(e => e.World)
                .WithMany(w => w.Characters)
                .HasForeignKey(e => e.WorldId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
