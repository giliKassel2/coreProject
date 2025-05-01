using myProject.Interfaces;
using myProject.Middlewares;
using myProject.Servise;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

void ConfigureServices(IServiceCollection services)
{
  services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(cfg =>
  {
      cfg.RequireHttpsMetadata = false;
      cfg.TokenValidationParameters = 
          FbiTokenService.GetTokenValidationParameters();
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
}

// Add services to the container
builder.Services.AddScoped<IClasService, ClassService>();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // app.MapOpenApi();
}

app.UseLog();

app.UseError();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
