using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Configurations
{
    public class GraphNodeConfiguration : IEntityTypeConfiguration<GraphNode>
    {
        public void Configure(EntityTypeBuilder<GraphNode> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.X).IsRequired();
            builder.Property(n => n.Y).IsRequired();

            builder.Property(n => n.CreatedAt).IsRequired();
            builder.Property(n => n.UpdatedAt).IsRequired();

            builder.HasOne(n => n.Graph)
                .WithMany(g => g.Nodes)
                .HasForeignKey(n => n.GraphId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(n => n.Entity)
                .WithMany()
                .HasForeignKey(n => n.EntityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(n => new { n.GraphId, n.EntityId }).IsUnique();
        }
    }
}
