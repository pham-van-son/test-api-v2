using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TestLab.Entities;
using TestLab.IRepositories;
using TestLab.Mapper;
using TestLab.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

/// <summary>
/// Program.cs - Điểm khởi chạy của ứng dụng
/// Cấu hình các dịch vụ (services) và middleware cho API.
/// Tác giả: [SonPV]
/// Ngày sửa: [22-09-2025]
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Cấu hình Services

        // Thêm controllers
        builder.Services.AddControllers();

        // Cấu hình Swagger / OpenAPI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "TestLab API",
                Version = "v1"
            });
        });

        // Cấu hình Entity Framework với SQL Server
        builder.Services.AddDbContext<Gps3LabContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));

        // Cấu hình AutoMapper
        builder.Services.AddAutoMapper(config =>
        {
            config.AddProfile<AutoMapperProfile>();
        });

        // Đăng ký Repository
        builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
        builder.Services.AddScoped<IVehicleGroupRepository, VehicleGroupRepository>();

        // Đăng ký các dịch vụ HTTP
        builder.Services.AddHttpClient();
        builder.Services.AddHttpContextAccessor();

        // Cấu hình Authentication (JWT)
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

        builder.Services.AddAuthorization();

        // Cấu hình CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        #endregion

        var app = builder.Build();

        #region Cấu hình Middleware

        // Cho phép CORS
        app.UseCors("AllowAll");

        // Bắt buộc HTTPS
        app.UseHttpsRedirection();

        // Authentication & Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Map API controllers
        app.MapControllers();

        // Bật Swagger UI
        app.UseSwagger();
        app.UseSwaggerUI();

        #endregion

        app.Run();
    }
}
