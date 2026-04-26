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
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.FromType)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(r => r.ToType)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(r => r.CreatedAt).IsRequired();

            builder.HasOne(r => r.World)
                .WithMany(w => w.Relations)
                .HasForeignKey(r => r.WorldId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(r => new { r.WorldId, r.FromId, r.FromType });
            builder.HasIndex(r => new { r.WorldId, r.ToId, r.ToType });
        }
    }
}
