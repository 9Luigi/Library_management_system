using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    internal class EntityConfigurationForAuthor:IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            builder.Property(a => a.Surname).HasColumnName("Surname").IsRequired().HasMaxLength(50);
            builder.Property(a => a.Biography).HasColumnName("Biography").HasMaxLength(500);
            builder.Property(a => a.Photo).HasColumnName("Photo");
            builder.Property(a => a.AuthorVersion).HasColumnName("Version").IsConcurrencyToken().IsRequired();
        }
    }
}
