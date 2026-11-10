using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations
{
    internal class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(m => m.Id);
            builder.HasIndex(m => m.IIN).IsUnique();
            builder.HasMany(m => m.Books).WithMany(b => b.Members);

            builder.Property(m => m.Id).ValueGeneratedOnAdd();
            builder.Property(m => m.Name).HasColumnName("Name").IsRequired().HasMaxLength(75);
            builder.Property(m => m.Surname).HasColumnName("Surname").IsRequired().HasMaxLength(75);
            builder.Property(m => m.Patronymic).HasColumnName("Patronymic").HasMaxLength(75);
            builder.Property(m => m.Age).HasColumnName("Age").IsRequired();
            builder.Property(m => m.BirthDay).HasColumnName("BirthDay").IsRequired();
            builder.Property(m => m.IIN).HasColumnName("IIN").IsRequired();
            builder.Property(m => m.PhotoPath).HasColumnName("PhotoPath").HasMaxLength(260).IsRequired(false);
            builder.Property(m => m.RegistrationDate).HasColumnName("RegistrationDate").IsRequired();
            builder.Property(m => m.Adress).HasColumnName("Adress").IsRequired();
            builder.Property(m => m.PhoneNumber).HasColumnName("PhoneNumber").IsRequired().HasMaxLength(75);
        }
    }
}