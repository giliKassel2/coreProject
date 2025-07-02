using myProject.Models;
using myProject.Interfaces;
using myProject.Controllers;
using System.Text.Json;
using System.Security.Claims;

namespace myProject.Services;

    public class TeacherService:GenericService<Teacher>{
        private readonly string _filePath = "Data/TeacherData.json";
        private List<Teacher> _entities;
        private readonly IHttpContextAccessor _httpContextAccessor;
    public TeacherService(IHostEnvironment env)
    {
        // Load entities from JSON file
        if (File.Exists(_filePath))
        {
            using (var json = File.OpenText(_filePath))
            {
                _entities = JsonSerializer.Deserialize<List<Teacher>>(json.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<Teacher>();
            }
            System.Console.WriteLine(_entities);
        }
        else
        {
            _entities = new List<Teacher>();
        }
        var userType = _httpContextAccessor.HttpContext?.User?.FindFirst("type")?.Value;
        if (userType == "Teacher")
        {
            _entities = _entities.Where(t => t.Id == int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.PrimarySid)?.Value ?? "0")).ToList();
        }
        else if (userType == "principal")
        {
            // No filtering for principal
        }
        else
        {
            _entities = new List<Teacher>();
        }
    }
        
        public Teacher? Create(Teacher teacher)
        {
            teacher.HashPassword = PasswordService.HashPassword(teacher.HashPassword);
           return base.Create(teacher);
        }
   
    }
