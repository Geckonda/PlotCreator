using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Configurations
{
    public class GraphConfiguration : IEntityTypeConfiguration<Graph>
    {
        public void Configure(EntityTypeBuilder<Graph> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(g => g.Kind)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(g => g.IsSystemDefault)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(g => g.CreatedAt).IsRequired();
            builder.Property(g => g.UpdatedAt).IsRequired();

            builder.HasOne(g => g.Owner)
                .WithMany()
                .HasForeignKey(g => g.OwnerUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(g => g.World)
                .WithMany()
                .HasForeignKey(g => g.WorldId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(g => new { g.WorldId, g.OwnerUserId });

            builder.HasIndex(g => new { g.OwnerUserId, g.WorldId })
                .HasFilter("\"IsSystemDefault\" = TRUE")
                .IsUnique()
                .HasDatabaseName("IX_Graphs_DefaultPerUserWorld");
        }
    }
}
