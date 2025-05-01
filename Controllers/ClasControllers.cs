
using Microsoft.AspNetCore.Mvc;
using MyProject.models;
using myProject.Servise;
//using Microsoft.AspNetCore.Http.HttpResults;
using myProject.Interfaces;
namespace myProject.Controllers;

[ApiController]
[Route("[controller]")]

public class ClasControllers : ControllerBase{
      private IClasService clasService;

    public ClasControllers(IClasService clasService)
    {
        this.clasService = clasService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Student>> Get()
    {
        
        return clasService.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<Student> Get(int id){
        Student s= clasService.Get(id);
        if(s == null)
            return NotFound();

        return s;
    }

    [HttpPost]
    public ActionResult Post(Student newStudent){
      
        int newId = clasService.Insert(newStudent);
        if (newId == -1)
            return BadRequest();

        return CreatedAtAction(nameof(Post) , new {Id = newId});

    }


    [HttpPut("{id}")]

    public ActionResult Put(int id ,Student newStudent)
    {
        if(clasService.UpDate(id , newStudent))
            return NoContent();

        return BadRequest();
        
    }

    [HttpDelete("{id}")]

    public ActionResult Delete(int id)
    {
        if( clasService.Delete(id)){
                return Ok();
        }
        return NotFound();
    }

}