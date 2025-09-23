using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using test_lab.Entities;
using test_lab.IRepositories;
using test_lab.Mapper;
using test_lab.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// C?u hình k?t n?i SQL Server v?i Dapper
builder.Services.AddScoped<IDbConnection>(sp =>
{
    return new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// C?u hình k?t n?i SQL Server v?i Entity Framework Core
builder.Services.AddDbContext<Gps3LabContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(config =>
{
  config.AddProfile<AutoMapperProfile>();
});

builder.Services.AddHttpClient();

builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleGroupRepository, VehicleGroupRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAll", policy =>
  {
    policy.WithOrigins()
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .SetIsOriginAllowed(origin => true)
          .AllowAnyHeader();
  });
});

var app = builder.Build();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
