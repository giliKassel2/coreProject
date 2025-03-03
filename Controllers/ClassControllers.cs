using Microsoft.AspNetCore.Mvc;
using MyProject.models;
using myProject.Servise;
using Microsoft.AspNetCore.Http.HttpResults;

namespace myProject.Controllers;

[ApiController]
[Route("[controller]")]
public class clasControllers : ControllerBase{

    [HttpGet]
    public ActionResult<IEnumerable<Student>> Get()
    {
        return ClassService.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<Student> Get(int id){
        Student s= ClassService.Get(id);
        if(s == null)
            return NotFound();

        return s;
    }

    [HttpPost]
    public ActionResult Post(Student newStudent){
        int newId = ClassService.Insert(newStudent);
        if (newId == -1)
            return BadRequest();

        return CreatedAtAction(nameof(Post) , new {Id = newId});

    }


    [HttpPut("{id}")]

    public ActionResult Put(int id ,Student newStudent)
    {
        if(ClassService.UpDate(id , newStudent))
            return NoContent();

        return BadRequest();
        
    }

    [HttpDelete("{id}")]

    public ActionResult Delete(int id)
    {
        if( ClassService.Delete(id))
          return Ok();

        return NotFound();
    }

}