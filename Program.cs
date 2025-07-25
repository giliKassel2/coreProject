using myProject.Interfaces;
using myProject.Middlewares;
using myProject.Services;
using myProject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using myProject.Controllers;
using System.IO;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
 builder.Services.AddSingleton<IHostEnvironment>(builder.Environment);
// Configure services
void ConfigureServices(IServiceCollection services)
{
   
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.TokenValidationParameters = 
            CreateTokenService.GetTokenValidationParameters();

        cfg.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var tokenFromCookie = context.Request.Cookies["AuthToken"];
                if (!string.IsNullOrEmpty(tokenFromCookie))
                {
                    context.Token = tokenFromCookie;
                }
                return Task.CompletedTask;
            }
        };
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

    services.AddScoped<StudentService>();
    services.AddScoped<TeacherService>();
    services.AddScoped<LoginService>();
    services.AddScoped<UserContextService>();

    services.AddScoped<IGenericService<Student>>(provider =>
{
    var env = provider.GetRequiredService<IHostEnvironment>();
    return new GenericService<Student>(
        JsonManageService<Student>.LoadFromJson(
            Path.Combine(env.ContentRootPath, "Data", "StudentData.json")
        ),
        Path.Combine(env.ContentRootPath, "Data", "StudentData.json")
    );
});

services.AddScoped<IGenericService<Teacher>>(provider =>
{
    var env = provider.GetRequiredService<IHostEnvironment>();
    return new GenericService<Teacher>(
        JsonManageService<Teacher>.LoadFromJson(
            Path.Combine(env.ContentRootPath, "Data", "TeacherData.json")
        ),
        Path.Combine(env.ContentRootPath, "Data", "TeacherData.json")
    );
});

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tasks", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIs...\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "Bearer",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new string[] {}
            }
        });
    });
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
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/login.html");
        return;
    }
    await next();
});
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "login.html", "index.html" }
});
app.UseStaticFiles();
app.UseLog();
app.UseError();
app.UseHttpsRedirection();

// Add UseRouting before UseAuthentication and UseAuthorization
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
// Map controllers
app.MapControllers();

// Map default route for login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
);

// Run the application
app.Run();
