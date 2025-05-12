// using Microsoft.AspNetCore.Mvc;
// using myProject.Interfaces;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// namespace myProject.Controllers;

// [ApiController]
// [Route("api/[controller]")]
// public class GenericController<T> : ControllerBase
// {
//     private readonly IGenericService<T> _service;

//     public GenericController(IGenericService<T> service)
//     {
//         _service = service;
//     }

//     [HttpGet]
//     public async Task<ActionResult<IEnumerable<T>>> Get()
//     {
//         var items =  _service.Get();
//         return Ok(items);
//     }

//     [HttpGet("{id}")]
//     public async Task<ActionResult<T>> Get(Func<T, bool> predicate)
//     {
//         try
//         {
//             var item =  _service.Get(predicate);
//             return Ok(item);
//         }
//         catch
//         {
//             return NotFound();
//         }
//     }

//     [HttpPost]
//     public async Task<ActionResult<T>> Create([FromBody] T entity)
//     {
//         var createdItem =  _service.Create(entity);
//         return CreatedAtAction(nameof(Get), new { id = createdItem }, createdItem);
//     }

//     [HttpPut("{id}")]
//     public  Task<ActionResult<T>> Update(Func<T, bool> predicate, [FromBody] T entity)
//     {
//         try
//         {
//             var updatedItem =  _service.Update(predicate);
//             return Ok(updatedItem);
//         }
//         catch
//         {
//             return NotFound();
//         }
//     }

//     [HttpDelete("{id}")]
//     public async Task<IActionResult> Delete(Func<T, bool> predicate)
//     {
//         try
//         {
//              _service.Delete(predicate);
//             return NoContent();
//         }
//         catch
//         {
//             return NotFound();
//         }
//     }
// }