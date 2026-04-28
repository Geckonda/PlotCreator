using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Configurations
{
    public class EntityTypeConfiguration : IEntityTypeConfiguration<EntityType>
    {
        public void Configure(EntityTypeBuilder<EntityType> builder)
        {
            builder.ToTable("EntityTypes");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Key)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.Label)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Color).HasMaxLength(9);
            builder.Property(t => t.Icon).HasMaxLength(50);

            builder.Property(t => t.Radius).IsRequired();
            builder.Property(t => t.IsSystemDefault).IsRequired();

            builder.Property(t => t.PropertySchemaJson)
                .HasColumnName("PropertySchema")
                .HasColumnType("jsonb")
                .IsRequired();

            builder.Property(t => t.CreatedAt).IsRequired();
            builder.Property(t => t.UpdatedAt).IsRequired();

            builder.HasOne(t => t.Owner)
                .WithMany(u => u.EntityTypes)
                .HasForeignKey(t => t.OwnerUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(t => new { t.OwnerUserId, t.Key }).IsUnique();
        }
    }
}
