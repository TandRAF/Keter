using Microsoft.AspNetCore.Mvc;
using server.Services;
using server.Dtos.BoardDto;
using server.Interfaces;

namespace server.Controllers
{
    [ApiController]
    [Route("api/board")]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _boardService;

        public BoardController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BoardReadDto>> GetBoard(Guid id)
        {
            var board = await _boardService.GetBoardDataAsync(id);
            if (board == null) return NotFound("Board not found.");

            return Ok(board);
        }
    }
}