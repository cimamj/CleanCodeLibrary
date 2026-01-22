using CleanCode.Infrastructure.Database;
using CleanCode.Infrastructure.Repositories;
using CleanCodeLibrary.Application.Students.Student;
using CleanCodeLibrary.Application.Books.Book;
using CleanCodeLibrary.Domain.Persistance.Common;
using CleanCodeLibrary.Domain.Persistance.Students;
using CleanCodeLibrary.Domain.Persistance.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CleanCodeLibrary.Domain.Persistance.Borrows;

namespace CleanCode.Infrastructure
{
    public static class DependencyInjection //potreban za API sloj
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            AddDatabase(services, configuration);
            AddRepositories(services);

            //ako dapper
            AddDapper(services);

            return services;
        }

        private static void AddDatabase(
            IServiceCollection services,
            IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("Database");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(connectionString);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));
        }

        private static void AddRepositories(IServiceCollection services)
        {

            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>)); //za CRUD metode, fromservices IRepository<Student, int> repo ti daje  Repository<Student, int> koji ima samo osnovne CRUD operacije, ali glupo opet, ugl ovo je da radi s bilo kojim entitetom
            services.AddScoped<IStudentRepository, StudentRepository>(); //za specificne metode, ali i ovo mozes koristiti za CRUD
                                                                         //jel ovo gori za sad dovoljno za moje metode
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBorrowRepository, BorrowRepository>(); 
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void AddDapper(IServiceCollection services)
        {
            services.AddScoped<IDapperManager, DapperManager>();
        }
    }
}
//je li ovo dobro pitaj ivu

