using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.BoardDto;
using server.Interfaces;

namespace server.Controllers
{
    [Authorize]
    [Route("api/board")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _service;

        public BoardController(IBoardService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BoardReadDto>> GetById(Guid id)
        {
            var board = await _service.GetBoardByIdAsync(id);
            if (board == null) return NotFound(new { Message = "Board-ul nu a fost găsit." });
            
            return Ok(board);
        }

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<BoardReadDto>>> GetByProjectId(Guid projectId)
        {
            var boards = await _service.GetBoardsByProjectAsync(projectId);
            return Ok(boards);
        }
        [HttpPost]
        public async Task<ActionResult<BoardReadDto>> CreateBoard([FromBody] BoardCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var createdBoard = await _service.CreateBoardAsync(dto);
                return Ok(createdBoard);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"Eroare la crearea board-ului: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoard(Guid id)
        {
            try
            {
                var success = await _service.DeleteBoardAsync(id);
                if (!success) return NotFound(new { Message = "Board-ul nu a fost găsit sau a fost deja șters." });

                return NoContent();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"Eroare la ștergerea board-ului: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}