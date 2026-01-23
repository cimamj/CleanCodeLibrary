using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CleanCodeLibrary.Domain.Entities.Books;

namespace CleanCode.Infrastructure.Database.Configurations.Books
{
    internal sealed class BookConfiguration : IEntityTypeConfiguration<Book> 
    {
        public void Configure(EntityTypeBuilder<Book> builder)  
        {
            builder.ToTable("Books"); 

            builder.HasKey(t => t.Id); //properti naseg entiteta
            builder.Property(t => t.Id)
                .HasColumnName("Id") //properti nase tablice
                .ValueGeneratedOnAdd();  // ← važno za SERIAL

            builder.Property(t => t.Title)
                .HasColumnName("Title")
                .HasMaxLength(200) //
                .IsRequired(); //

            builder.Property(t => t.Author)
                .HasColumnName("Author")
                .HasMaxLength(150) //
                .IsRequired(); //

            builder.Property(t => t.Year)
                .HasColumnName("Year");

            builder.Property(t => t.Amount)
            .HasColumnName("Amount"); //moras rucno dodati u sql



        }
    }
}
