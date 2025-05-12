
// using Microsoft.AspNetCore.Mvc;
// using MyProject.Models;
// using myProject.Servise;
// using myProject.Interfaces;
// using myProject.Models;
// namespace myProject.Controllers;

// [ApiController]
// [Route("[controller]")]

// public class SchoolControllers : ControllerBase{
//       private ISchoolService clasService;

//     public SchoolControllers(ISchoolService clasService)
//     {
//         this.clasService = clasService;
//     }
//     [HttpGet]
//     public ActionResult<IEnumerable<Teacher>> Get()
//     {
        
//         return clasService.GetTeachers();
//     }
//     // [HttpGet]
//     // public ActionResult<IEnumerable<Student>> Get()
//     // {
        
//     //     return clasService.GetStudents();
//     // }

//     [HttpGet("{id}")]
//     public ActionResult<Student> Get(int id){
//         Student s= clasService.Get(id);
//         if(s == null)
//             return NotFound();

//         return s;
//     }

//     [HttpPost]
//     public ActionResult Post(Student newStudent){
      
//         int newId = clasService.Insert(newStudent);
//         if (newId == -1)
//             return BadRequest();

//         return CreatedAtAction(nameof(Post) , new {Id = newId});

//     }


//     [HttpPut("{id}")]

//     public ActionResult Put(int id ,Student newStudent)
//     {
//         if(clasService.UpDate(id , newStudent))
//             return NoContent();

//         return BadRequest();
        
//     }

//     [HttpDelete("{id}")]

//     public ActionResult Delete(int id)
//     {
//         if( clasService.Delete(id)){
//                 return Ok();
//         }
//         return NotFound();
//     }

// }