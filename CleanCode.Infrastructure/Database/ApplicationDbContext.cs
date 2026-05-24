using Microsoft.EntityFrameworkCore;
using CleanCodeLibrary.Domain.Entities.Students;
using CleanCodeLibrary.Domain.Entities.Books;
using CleanCodeLibrary.Domain.Entities.Borrows;

namespace CleanCode.Infrastructure.Database
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students
        {
            get; set;
        }

        public DbSet<Book> Books
        {
            get; set;
        }

        public DbSet<Borrow> Borrows
        {
            get; set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.HasDefaultSchema(Schemas.Default);
        }
    }
}
