using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.ColumnDto;
using server.Interfaces;

namespace server.Controllers
{
    [Authorize]
    [Route("api/column")]
    [ApiController]
    public class ColumnController : ControllerBase
    {
        private readonly IColumnService _service;

        public ColumnController(IColumnService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<ColumnReadDto>> CreateColumn([FromBody] ColumnCreateDto dto)
        {
            var newColumn = await _service.CreateColumnAsync(dto);
            return CreatedAtAction(nameof(CreateColumn), new { id = newColumn.Id }, newColumn); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateColumn(Guid id, [FromBody] ColumnUpdateDto dto)
        {
            var success = await _service.UpdateColumnAsync(id, dto);
            if (!success) return NotFound(new { Message = "Column not found." });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColumn(Guid id)
        {
            var success = await _service.DeleteColumnAsync(id);
            if (!success) return NotFound(new { Message = "Column not found." });

            return NoContent();
        }
    }
}