using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Configurations
{
    public class GraphEdgeConfiguration : IEntityTypeConfiguration<GraphEdge>
    {
        public void Configure(EntityTypeBuilder<GraphEdge> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Direction)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.Label).HasMaxLength(200);

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();

            builder.HasOne(e => e.Graph)
                .WithMany(g => g.Edges)
                .HasForeignKey(e => e.GraphId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.FromNode)
                .WithMany()
                .HasForeignKey(e => e.FromNodeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.ToNode)
                .WithMany()
                .HasForeignKey(e => e.ToNodeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.GraphId);
            builder.HasIndex(e => new { e.GraphId, e.FromNodeId, e.ToNodeId });
        }
    }
}
