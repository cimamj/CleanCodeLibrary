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

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(b => b.StudentId)
                .HasColumnName("studentid")
                .IsRequired();

            builder.HasOne<Student>(b => b.Student)
                .WithMany(s => s.Borrows)
                .HasForeignKey(b => b.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(b => b.BookId)
                .HasColumnName("bookid")
                .IsRequired();

            builder.HasOne<Book>(b => b.Book)
                .WithMany(b => b.Borrows)
                .HasForeignKey(b => b.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(b => b.BorrowDate)
                .HasColumnName("borrowdate")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            builder.Property(b => b.DueDate)
                .HasColumnName("duedate")
                .IsRequired();

            builder.Property(b => b.ReturnDate)
                .HasColumnName("returndate")
                .IsRequired(false);

            builder.Property(t => t.AmountBorrowed)
                .HasColumnName("amountborrowed")
                .IsRequired();
        }
    }
}
