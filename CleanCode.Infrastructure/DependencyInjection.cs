using CleanCode.Infrastructure.Database;
using CleanCode.Infrastructure.Repositories;
using CleanCode.Infrastructure.Services;
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Domain.Persistance.Books;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using CleanCodeLibrary.Domain.Persistance.Common;
using CleanCodeLibrary.Domain.Persistance.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CleanCode.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            AddDatabase(services, configuration);

            AddRepositories(services);

            AddDapper(services, configuration);

            AddServices(services);

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
            {
                options.UseNpgsql(connectionString)
                       .EnableSensitiveDataLogging()
                       .LogTo(Console.WriteLine, LogLevel.Information);
            });
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            services.AddScoped<IStudentRepository, StudentRepository>();

            services.AddScoped<IBookRepository, BookRepository>();

            services.AddScoped<IBorrowRepository, BorrowRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IBorrowUnitOfWork, BorrowUnitOfWork>();
        }

        private static void AddDapper(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            services.AddScoped<IDapperManager>(_ => new DapperManager(connectionString));
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        }
    }
}
