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
    internal class EntityConfigurationForMember:IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(m => m.Id);
            builder.HasIndex(m => m.Id).IsUnique();
            builder.HasIndex(m => m.IIN).IsUnique();
            builder.HasMany(m => m.Books).WithMany(b => b.Members);

            builder.Property(m => m.Name).HasColumnName("Name").IsRequired().HasMaxLength(75);
            builder.Property(m => m.Surname).HasColumnName("Surname").IsRequired().HasMaxLength(75);
            builder.Property(m => m.Patronymic).HasColumnName("Patronymic").HasMaxLength(75);
            builder.Property(m => m.Age).HasColumnName("Age").IsRequired();
            builder.Property(m => m.BirthDay).HasColumnName("BirthDay").IsRequired();
            builder.Property(m => m.IIN).HasColumnName("IIN").HasColumnType("BIGINT").IsRequired();
            builder.Property(m => m.Photo).HasColumnName("Photo").HasDefaultValue(new byte[1]).IsRequired();
            builder.Property(m => m.RegistrationDate).HasColumnName("RegistrationDate").IsRequired();
            builder.Property(m => m.Adress).HasColumnName("Adress").IsRequired();
            builder.Property(m => m.PhoneNumber).HasColumnName("PhoneNumber").IsRequired().HasMaxLength(75);
            builder.Property(m => m.MemberVersion).HasColumnName("Version").IsRowVersion();
        }

    }
}
