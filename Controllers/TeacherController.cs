using Microsoft.AspNetCore.Mvc;
using myProject.Models;
using myProject.Services;
using myProject.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace myProject.Controllers
{
    [Authorize(Policy = "principal")]
    [Route("api/[controller]")]
    public class TeachersController : ControllerBase
    {
              private TeacherService service;

        public TeachersController(TeacherService service){
            this.service = service;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Teacher>>? Get()
        {
            return service.Get();
        }
        
        [HttpGet("{id}")]
        public ActionResult<Teacher> Get(int id){
            Teacher s = service.Get(s => s.Id == id);
            if(s == null)
                return NotFound();
            return s;
        }

        [HttpPost]
        public ActionResult Post(Teacher newTeacher)
        {
            Teacher newT= service.Create(newTeacher);
             if (newT == default)
            {
                return BadRequest("Teacher creation failed. Please ensure the teacher has a valid ID and password.");
            }
            if (newT == null)
            {
                System.Console.WriteLine("Teacher creation failed. Please ensure the teacher has a valid ID and password.__2");
                 return BadRequest();
            }
               

            return CreatedAtAction(nameof(Post) , new {Id = newT.Id});
        }

   
        [HttpPut]
        public ActionResult Put(Teacher newTeacher)
        {
            if(service.Update(newTeacher ,s =>s.Id == newTeacher.Id) == default)
                return NotFound();

            return Ok( newTeacher.Id);
            
        }

        

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
