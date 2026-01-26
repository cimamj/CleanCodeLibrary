using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CleanCodeLibrary.Domain.Entities.Books;

namespace CleanCode.Infrastructure.Database.Configurations.Books
{
    internal sealed class BookConfiguration : IEntityTypeConfiguration<Book> 
    {
        public void Configure(EntityTypeBuilder<Book> builder)  
        {
            builder.ToTable("books"); 

            builder.HasKey(t => t.Id); //properti naseg entiteta
            builder.Property(t => t.Id)
                .HasColumnName("id") //properti nase tablice
                .ValueGeneratedOnAdd();  // ← važno za SERIAL

            builder.Property(t => t.Title)
                .HasColumnName("title")
                .HasMaxLength(200) //
                .IsRequired(); //

            builder.Property(t => t.Author)
                .HasColumnName("author")
                .HasMaxLength(150) //
                .IsRequired(); //

            builder.Property(t => t.Year)
                .HasColumnName("year");

            builder.Property(t => t.Amount)
            .HasColumnName("amount"); //moras rucno dodati u sql

            builder.Property(t => t.Genre)
      .HasColumnName("genre");

        }
    }
}
