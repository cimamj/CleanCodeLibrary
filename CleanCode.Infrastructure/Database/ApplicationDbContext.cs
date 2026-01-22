using Microsoft.EntityFrameworkCore;
using CleanCodeLibrary.Domain.Entities.Students;
using CleanCodeLibrary.Domain.Entities.Books;

namespace CleanCode.Infrastructure.Database
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

  
        public DbSet<Student> Students {  get; set; } //s ovim poljem se pristupa ugradenim funkcijama za query
        public DbSet<Book> Books { get; set; }
        // itd.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.HasDefaultSchema(Schemas.Default); 
        }
    }

}
