using Library.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Server.Data.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
	public void Configure(EntityTypeBuilder<Member> builder)
	{
		builder.HasKey(m => m.Id);
		builder.HasIndex(m => m.IIN).IsUnique();
		builder.Property(m => m.IIN).IsRequired().HasMaxLength(12);

		builder.Property(m => m.Name).IsRequired().HasMaxLength(75);
		builder.Property(m => m.Surname).IsRequired().HasMaxLength(75);
		builder.Property(m => m.Patronymic).HasMaxLength(75);
		builder.Property(m => m.Address).IsRequired();
		builder.Property(m => m.PhoneNumber).IsRequired().HasMaxLength(75);

		// PostgreSQL maps DateTime to "timestamp with time zone", which accepts only UTC values.
		// A birthday is just a date, and the registration time is local (DateTime.Now)
		builder.Property(m => m.BirthDay).HasColumnType("date");
		builder.Property(m => m.RegistrationDate).HasColumnType("timestamp without time zone");
	}
}
