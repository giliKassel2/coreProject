using Microsoft.AspNetCore.Mvc;
using myProject.Models;
using myProject.Services;
using myProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace myProject.Controllers
{
    [Route("api/[controller]")]
    public class TeachersController : ControllerBase
    {
        private TeacherService service;

        public TeachersController(TeacherService service)
        {
            this.service = service;
        }

        [Authorize(Policy = "principal")]
        [HttpGet]
        public ActionResult<IEnumerable<Teacher>>? Get()
        {
            if (service.Get() != null)
                return service.Get();
            return NotFound();
        }

        [Authorize(Policy = "teacher")]
        [HttpGet("{id}")]
        public ActionResult<Teacher> Get(int id)
        {
            Teacher s = service.Get(s => s.Id == id);
            if (s == null)
                return NotFound();
            return s;
        }

        [Authorize(Policy = "principal")]
        [HttpPost]
        public ActionResult Post(Teacher newTeacher)
        {
            Teacher newT = service.Create(newTeacher);
            if (newT == null)
                return BadRequest();

            return CreatedAtAction(nameof(Post), new { Id = newT.Id });
        }

        [Authorize(Policy = "principal")]
        [HttpPut("{id}")]
        public ActionResult Put(int id, Teacher newTeacher)
        {
            System.Console.WriteLine("id = " + id);

            if (service.Update(newTeacher, s => s.Id == id) == default(Teacher))

                return NotFound();

            return CreatedAtAction(nameof(Put), newTeacher);

        }

        [Authorize(Policy = "principal")]
        [HttpDelete("{id}")]

        public ActionResult Delete(int id)
        {
            if (service.Delete(s => s.Id == id))
            {
                return Ok();
            }
            return NotFound();
        }
            
            
        [Authorize(Policy = "teacher")]
        [HttpGet("me")]
        public ActionResult<Teacher> GetCurrentStudent()
        {
            System.Console.WriteLine("start me http get");
            var userIdClaim = User.FindFirst(ClaimTypes.PrimarySid)?.Value;
            System.Console.WriteLine(userIdClaim);
            if (!int.TryParse(userIdClaim, out int userId))
            {
                System.Console.WriteLine("Unauthorized");
                return Unauthorized();
            }
                
            var teacher = service.Get(s => s.Id == userId);
            if (teacher == null)
                return NotFound();
            
            return teacher;
        }
        }
    }
