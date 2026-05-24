using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CleanCodeLibrary.Domain.Entities.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCode.Infrastructure.Database.Configurations.Users
{
    internal sealed class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("students");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(t => t.FirstName)
                .HasColumnName("firstname")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.LastName)
                .HasColumnName("lastname")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.DateOfBirth)
                .HasColumnName("dateofbirth");

            builder.Property(t => t.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(s => s.Email)
                .IsUnique();

            builder.Property(s => s.PasswordHash)
                .HasColumnName("password_hash")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(s => s.Role)
                .HasColumnName("role")
                .HasMaxLength(50)
                .IsRequired()
                .HasDefaultValue("Student");
        }
    }
}
