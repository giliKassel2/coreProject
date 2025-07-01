using Microsoft.AspNetCore.Mvc;
using myProject.Models;
using myProject.Services;
using myProject.Interfaces;

namespace myProject.Controllers
{
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private StudentService service;

        public StudentController(StudentService service)
        {
            this.service = service;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Student>> Get()
        {
            return service.Get();
        }
        [HttpGet("{id}")]
        public ActionResult<Student> Get(int id)
        {
            Student s = service.Get(s => s.Id == id);
            if (s == null)
                return NotFound();

            return s;
        }

        [HttpPost]
        public ActionResult Post(Student newStudent)
        {
            Student newS = service.Create(newStudent);
            if (newS == default)
            {
                return BadRequest("Student creation failed. Please ensure the student has a valid ID and password.");
            }
            if (newS == null)
                return BadRequest();

            return CreatedAtAction(nameof(Post), new { Id = newS.Id });
        }


        [HttpPut]
        public ActionResult Put(Student newStudent)
        {
            if (service.Update(newStudent, s => s.Id == newStudent.Id) == default)
                return NotFound();

            return Ok(newStudent.Id);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (service.Delete(s => s.Id == id))
            {
                return Ok();
            }
            return NotFound();
        }

    }
}