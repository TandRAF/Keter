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
    }
}