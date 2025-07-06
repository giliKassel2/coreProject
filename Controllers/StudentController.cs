using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using myProject.Interfaces;
using myProject.Models;
using myProject.Services;

namespace myProject.Controllers
{
    [Route("api/[controller]")]
    public class StudentController: ControllerBase
    {
        private StudentService service;

        public StudentController(StudentService service){
            this.service = service;
        }

        [Authorize(Policy = "teacher")]
        [HttpGet]
        public ActionResult<IEnumerable<Student>> Get()
        {
            if (service.Get()!= null)
                return service.Get();
            return NotFound();
        }

        [Authorize(Policy = "teacher")]
        [HttpGet("{id}")]
        public ActionResult<Student> Get(int id){
            Student s = service.Get(s => s.Id == id);
            if(s == null)
                return NotFound();

            return s;
        }

        [Authorize(Policy = "principal")]
        [HttpPost]
        public ActionResult Post(Student newStudent)
        {
            Student newS= service.Create(newStudent);
            if (newS == null)
                return BadRequest();

            return CreatedAtAction(nameof(Post) , new {Id = newS.Id});
        }

        [Authorize(Policy = "teacher")]
        [HttpPut("{id}")]
        public ActionResult Put(int id ,Student newStudent)
        {
            if(service.Update(newStudent ,s =>s.Id == id) == default(Student))
                return NotFound();

            return CreatedAtAction(nameof(Put) , newStudent);
            
        }

        [Authorize(Policy = "principal")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if(service.Delete(s =>s.Id == id)){
                    return Ok();
            }
            return NotFound();
        }
            
        }
}