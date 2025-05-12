using myProject.Interfaces;
using myProject.Middlewares;
using myProject.Services;
using myProject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using myProject.Controllers;
using System.IO;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Configure services
void ConfigureServices(IServiceCollection services)
{
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.TokenValidationParameters = 
            CreateTokenService.GetTokenValidationParameters();
    });

    services.AddAuthorization(cfg =>
    {
        cfg.AddPolicy("principal", 
            policy => policy.RequireClaim("type", "principal"));
        cfg.AddPolicy("teacher", 
            policy => policy.RequireClaim("type", "Teacher", "principal"));
        cfg.AddPolicy("student", 
            policy => policy.RequireClaim("type", "Student", "Teacher", "principal"));
    });
    // // Add services to the container
    // services.AddScoped<IClasService, ClassService>();
    services.AddScoped<StudentService>();
    services.AddScoped<TeacherService>();



    // Register generic service for students
    services.AddScoped<IGenericService<Student>>(provider =>
        new GenericService<Student>(
            Path.Combine(builder.Environment.ContentRootPath, "Data", "students.json")
        ));

    // Register generic service for teachers
    services.AddScoped<IGenericService<Teacher>>(provider =>
        new GenericService<Teacher>(
            Path.Combine(builder.Environment.ContentRootPath, "Data", "teachers.json")
        ));

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddControllers();

}

// Call ConfigureServices before building the app
ConfigureServices(builder.Services);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseLog();
app.UseError();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
