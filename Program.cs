using Microsoft.EntityFrameworkCore;
using CrudStudents.Data;
using CrudStudents.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Base de datos en memoria
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("UsersDB"));

var app = builder.Build();

// Middleware de logging
app.UseMiddleware<LoggingMiddleware>();

// Middleware de autenticación
app.UseMiddleware<AuthMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();