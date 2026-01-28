using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CleanCodeLibrary.Domain.Entities.Borrows;
using CleanCodeLibrary.Domain.Entities.Students;
using CleanCodeLibrary.Domain.Entities.Books;

namespace CleanCode.Infrastructure.Database.Configurations.Borrows
{
    internal sealed class BorrowConfiguration : IEntityTypeConfiguration<Borrow>
    {
        public void Configure(EntityTypeBuilder<Borrow> builder)
        {
            builder.ToTable("borrows");

            // Primarni ključ
            builder.HasKey(b => b.Id);

            // Id je SERIAL (auto-increment)
            builder.Property(b => b.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            // StudentId - NOT NULL, FK na Students
            builder.Property(b => b.StudentId)
                .HasColumnName("studentid")
                .IsRequired();

            builder.HasOne<Student>()                     // relacija prema Student
                .WithMany()                               // Student može imati više posudbi
                .HasForeignKey(b => b.StudentId)
                .OnDelete(DeleteBehavior.Cascade);        // ako se obriše student → brišu se i posudbe

            // BookId - NOT NULL, FK na Books
            builder.Property(b => b.BookId)
                .HasColumnName("bookid")
                .IsRequired();

            builder.HasOne<Book>()                        // relacija prema Book
                .WithMany()                               // Knjiga može biti posuđena više puta (ali ne istovremeno)
                .HasForeignKey(b => b.BookId)
                .OnDelete(DeleteBehavior.Cascade);        // ako se obriše knjiga → brišu se i posudbe

            // BorrowDate - DEFAULT CURRENT_TIMESTAMP
            builder.Property(b => b.BorrowDate)
                .HasColumnName("borrowdate")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            // DueDate - NOT NULL
            builder.Property(b => b.DueDate)
                .HasColumnName("duedate")
                .IsRequired();

            // ReturnDate - NULLABLE (može biti NULL dok nije vraćeno)
            builder.Property(b => b.ReturnDate)
                .HasColumnName("returndate")
                .IsRequired(false);

            builder.Property(t => t.AmountBorrowed)
           .HasColumnName("amountborrowed")
           .IsRequired();


        }
    }
}