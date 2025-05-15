using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using todo_api.DTOs;
using todo_api.Services;

namespace todo_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly ITodoService _service;

        public TodoController(ITodoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all ToDo tasks.
        /// </summary>
        /// <returns>200 OK with list of <see cref="TodoReadDto"/>.</returns>
        [HttpGet]
        public async Task<ActionResult<List<TodoReadDto>>> GetAll()
        {
            var todos = await _service.GetAllItemsAsync();
            return Ok(todos);
        }

        /// <summary>
        /// Retrieves a single ToDo task by its identifier.
        /// </summary>
        /// <param name="id">Task ID.</param>
        /// <returns>200 OK with <see cref="TodoReadDto"/> if found; 404 Not Found otherwise.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoReadDto>> GetById(int id)
        {
            var todo = await _service.GetByIdAsync(id);
            if (todo is null)
                return NotFound();
            return Ok(todo);
        }

        /// <summary>
        /// Retrieves incoming ToDo tasks (due today, next day or this week).
        /// </summary>
        [HttpGet("incoming")]
        public async Task<ActionResult<List<TodoReadDto>>> GetIncoming()
        {
            var todos = await _service.GetIncomingAsync();
            return Ok(todos);
        }

        /// <summary>
        /// Creates a new ToDo task.
        /// </summary>
        /// <param name="dto">Data for the new task.</param>
        /// <returns>201 Created with the created <see cref="TodoReadDto"/>.</returns>
        [HttpPost]
        public async Task<ActionResult<TodoReadDto>> Create([FromBody] TodoCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing ToDo task.
        /// </summary>
        /// <param name="id">Task ID to update.</param>
        /// <param name="dto">Updated data.</param>
        /// <returns>204 No Content if update succeeded; 404 Not Found otherwise.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TodoUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Deletes a ToDo task by its identifier.
        /// </summary>
        /// <param name="id">Task ID to delete.</param>
        /// <returns>204 No Content if deletion succeeded; 404 Not Found otherwise.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Marks a ToDo task as completed (sets percent to 100).
        /// </summary>
        /// <param name="id">Task ID to mark completed.</param>
        /// <returns>204 No Content if succeeded; 404 Not Found otherwise.</returns>
        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            var done = await _service.MarkAsCompleted(id);
            if (!done)
                return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Updates the percent complete of a ToDo task.
        /// </summary>
        /// <param name="id">Task ID.</param>
        /// <param name="percent">New percent value (0–100).</param>
        /// <returns>204 No Content if succeeded; 404 Not Found otherwise.</returns>
        [HttpPatch("{id}/percent/{percent}")]
        public async Task<IActionResult> SetPercent(int id, [Range(0, 100)] int percent)
        {
            var ok = await _service.SetPercentCompleteAsync(id, percent);
            if (!ok)
                return NotFound();
            return NoContent();
        }
    }
}
