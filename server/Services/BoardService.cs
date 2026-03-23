using AutoMapper;
using server.Dtos.BoardDto;
using server.Interfaces;
using server.Models;

namespace server.Services
{
    public class BoardService : IBoardService
    {
        private readonly IBoardRepository _repo;
        private readonly IMapper _mapper;

        public BoardService(IBoardRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BoardReadDto>> GetBoardsByProjectAsync(Guid projectId)
        {
            var boards = await _repo.GetAllByProjectIdAsync(projectId);
            return _mapper.Map<IEnumerable<BoardReadDto>>(boards);
        }

        public async Task<BoardReadDto?> GetBoardByIdAsync(Guid id)
        {
            var board = await _repo.GetByIdAsync(id);
            if (board == null) return null;
            
            return _mapper.Map<BoardReadDto>(board);
        }
    }
}