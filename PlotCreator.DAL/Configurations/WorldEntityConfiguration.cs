using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlotCreator.Domain.Entity;

namespace PlotCreator.DAL.Configurations
{
    public class WorldEntityConfiguration : IEntityTypeConfiguration<WorldEntity>
    {
        public void Configure(EntityTypeBuilder<WorldEntity> builder)
        {
            builder.ToTable("Entities");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.Tags).IsRequired();

            builder.Property(e => e.Aliases).IsRequired();

            builder.Property(e => e.PropertiesJson)
                .HasColumnName("Properties")
                .HasColumnType("jsonb")
                .IsRequired();

            builder.Property(e => e.ContentJson)
                .HasColumnName("Content")
                .HasColumnType("jsonb")
                .IsRequired();

            builder.Property(e => e.ExtraContentsJson)
                .HasColumnName("ExtraContents")
                .HasColumnType("jsonb")
                .HasDefaultValue("[]")
                .IsRequired();

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();

            builder.HasOne(e => e.World)
                .WithMany(w => w.Entities)
                .HasForeignKey(e => e.WorldId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Type)
                .WithMany(t => t.Entities)
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.WorldId);
            builder.HasIndex(e => e.TypeId);
        }
    }
}
