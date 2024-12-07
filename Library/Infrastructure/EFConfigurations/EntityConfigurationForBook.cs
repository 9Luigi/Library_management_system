using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.EFConfigurations
{
    internal class EntityConfigurationForBook : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasMany(b => b.Authors).WithMany(a => a.Books);
            builder.Property(b => b.Title).HasColumnName("Title").IsRequired().HasMaxLength(50);
            builder.Property(b => b.PublicationDate).HasColumnName("publicationYear").IsRequired();
            builder.Property(b => b.Sort).HasColumnName("Sort").HasMaxLength(25);
            builder.Property(b => b.Description).HasColumnName("Description").HasMaxLength(500);
            builder.Property(b => b.Genre).HasColumnName("Genre").IsRequired().HasMaxLength(25);
            builder.Property(b => b.CoverImage).HasColumnName("CoverImage").IsRequired();
            builder.Property(b => b.BookVersion).HasColumnName("Version").IsRowVersion().IsRequired();
        }
    }
}
