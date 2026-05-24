using CleanCodeLibrary.Application.Auth.Login;
using CleanCodeLibrary.Application.Books.Book;
using CleanCodeLibrary.Application.Borrows.Borrow;
using CleanCodeLibrary.Application.Students.Student;
using Microsoft.Extensions.DependencyInjection;

namespace CleanCodeLibrary.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateBookRequestHandler>();
            services.AddScoped<UpdateBookRequestHandler>();
            services.AddScoped<DeleteBookRequestHandler>();
            services.AddScoped<GetAllBooksRequestHandler>();
            services.AddScoped<CleanCodeLibrary.Application.Books.Book.GetByIdRequestHandler>();
            services.AddScoped<GetUsedGenresRequestHandler>();
            services.AddScoped<GetTopBooksRequestHandler>();

            services.AddScoped<CleanCodeLibrary.Application.Students.Student.GetByIdRequestHandler>();
            services.AddScoped<CreateStudentRequestHandler>();
            services.AddScoped<UpdateStudentRequestHandler>();
            services.AddScoped<DeleteStudentRequestHandler>();
            services.AddScoped<GetAllStudentsRequestHandler>();
            services.AddScoped<GetActiveBorrowsForStudentRequestHandler>();

            services.AddScoped<CreateBorrowAndUpdateBookAmountRequestHandler>();
            services.AddScoped<ReturnBookRequestHandler>();
            services.AddScoped<GetBorrowStatisticsRequestHandler>();

            services.AddScoped<LoginRequestHandler>();

            return services;
        }
    }
}
