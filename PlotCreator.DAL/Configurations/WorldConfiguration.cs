using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Configurations
{
    public class WorldConfiguration : IEntityTypeConfiguration<World>
    {
        public void Configure(EntityTypeBuilder<World> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(w => w.Genre).HasMaxLength(100);
            builder.Property(w => w.Color).HasMaxLength(9);

            builder.Property(w => w.CreatedAt).IsRequired();
            builder.Property(w => w.UpdatedAt).IsRequired();

            builder.HasOne(w => w.Owner)
                .WithMany(u => u.Worlds)
                .HasForeignKey(w => w.OwnerUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(w => w.OwnerUserId);
        }
    }
}
