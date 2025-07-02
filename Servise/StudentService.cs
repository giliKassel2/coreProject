using myProject.Models;
using myProject.Interfaces;
using myProject.Controllers;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Net.Http;

namespace myProject.Services;

public class StudentService : GenericService<Student>
{
    private readonly string _filePath = "Data/StudentData.json";
    private List<Student> _entities;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public StudentService(IHostEnvironment env, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        // Load entities from JSON file
        if (File.Exists(_filePath))
        {
            using (var json = File.OpenText(_filePath))
            {
                _entities = JsonSerializer.Deserialize<List<Student>>(json.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<Student>();
                
            }
            var userType = _httpContextAccessor.HttpContext?.User?.FindFirst("type")?.Value;
                if (userType == "Student")
                {
                    _entities = _entities.Where(s => s.Id == int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.PrimarySid)?.Value ?? "0")).ToList();
                }
                else if (userType == "Teacher")
                {
                    var classesClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("classes")?.Value;
                    var teacherClasses = classesClaim?.Split(',') ?? Array.Empty<string>();

                    _entities = _entities
                        .Where(student => teacherClasses.Contains(student.Class))
                        .ToList();

                }
                else if (userType == "principal")
                {
                    // No filtering for principal
                }
            System.Console.WriteLine(_entities);
        }
        
    }

    public Student? Create(Student student)
    {
        student.HashPassword = PasswordService.HashPassword(student.HashPassword);
        return base.Create(student);
    }
}
