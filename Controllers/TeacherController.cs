using Microsoft.AspNetCore.Mvc;
using myProject.Models;
using myProject.Services;
using myProject.Interfaces;

namespace myProject.Controllers
{
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
            if (newT == null)
                return BadRequest();

            return CreatedAtAction(nameof(Post) , new {Id = newT.Id});
        }


        [HttpPut("{id}")]
        public ActionResult Put(int id ,Teacher newTeacher)
        {
            if(service.Update(newTeacher ,s =>s.Id == id) == default(Teacher))
                return NotFound();

            return CreatedAtAction(nameof(Put) , newTeacher);
            
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
