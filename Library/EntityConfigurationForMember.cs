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
            builder.HasCheckConstraint("Age", "AGE>0 AND AGE<120");

            builder.Property(s => s.Name).HasColumnName("Name").IsRequired().HasMaxLength(75);
            builder.Property(s => s.Surname).HasColumnName("Surname").IsRequired().HasMaxLength(75);
            builder.Property(s => s.Patronymic).HasColumnName("Patronymic").HasMaxLength(75);
            builder.Property(s => s.Age).HasColumnName("Age").IsRequired();
            builder.Property(s => s.BirthDay).HasColumnName("BirthDay").IsRequired();
            builder.Property(s => s.IIN).HasColumnName("IIN").HasColumnType("BIGINT").IsRequired();
            builder.Property(s => s.Photo).HasColumnName("Photo").HasDefaultValue(new byte[1]).IsRequired();
            builder.Property(s => s.RegistrationDate).HasColumnName("RegistrationDate").IsRequired();
            builder.Property(s => s.Adress).HasColumnName("Adress").IsRequired();
            builder.Property(s => s.PhoneNumber).HasColumnType("BIGINT").HasColumnName("PhoneNumber");
        }

    }
}
