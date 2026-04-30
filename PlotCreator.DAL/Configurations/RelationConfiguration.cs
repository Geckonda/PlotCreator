using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Configurations
{
    public class RelationConfiguration : IEntityTypeConfiguration<Relation>
    {
        public void Configure(EntityTypeBuilder<Relation> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Label)
                .HasMaxLength(200);

            builder.Property(r => r.CreatedAt).IsRequired();

            builder.HasOne(r => r.World)
                .WithMany(w => w.Relations)
                .HasForeignKey(r => r.WorldId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.From)
                .WithMany()
                .HasForeignKey(r => r.FromId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.To)
                .WithMany()
                .HasForeignKey(r => r.ToId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(r => new { r.WorldId, r.FromId });
            builder.HasIndex(r => new { r.WorldId, r.ToId });
        }
    }
}
