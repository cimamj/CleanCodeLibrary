using CleanCode.Api.Middleware;
using CleanCode.Api.Services;
using CleanCode.Infrastructure;
using CleanCode.Infrastructure.ExternalServices;
using CleanCodeLibrary.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// faza 1 - konfiguracija , svi servisi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CleanCode Library API", Version = "v1" });
});
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService,CurrentUserService>();

// JWT - takoder prije Build()
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddMemoryCache(); 
builder.Services.AddScoped<IBookCacheService, BookCacheService>();
builder.Services.AddScoped(typeof(ICacheService<>), typeof(CacheService<>));
builder.Services.AddHttpClient<IBookExternalService, OpenLibraryService>();


//sve builder services ide prije BUILDa
var app = builder.Build(); //DI se zakljuca, kreira se app
//faza 2 pipeline middleware
app.UseMiddleware<GlobalExceptionMiddleware>(); //mora bit prvi

app.UseCors("AllowAll"); //na ovo pokazuje next pokazivac iz Global...

// SVE app. OVDJE - nakon Build()
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanCode Library v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();