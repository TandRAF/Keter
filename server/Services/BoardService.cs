using AutoMapper;
using server.Dtos.BoardDto;
using server.Dtos.ColumnDto;
using server.Dtos.TaskDto;
using server.Dtos.TagDto;
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
            return boards.Select(b => new BoardReadDto
            {
                Id = b.Id,
                Name = b.Name,
                Columns = b.Columns.Select(c => new ColumnReadDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Order = c.Order,
                    Tasks = c.Tasks.Select(t => new TaskReadDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Status = t.Status,
                        
                        // ---> AICI AM ADAUGAT MAPAREA TAG-URILOR <---
                        Tags = t.Tags != null ? t.Tags.Select(tag => new TagReadDto 
                        { 
                            Id = tag.Id, 
                            Name = tag.Name, 
                            ColorHex = tag.ColorHex 
                        }).ToList() : new List<TagReadDto>()

                    }).ToList()
                }).ToList()
            }).ToList();
        }

        public async Task<BoardReadDto?> GetBoardByIdAsync(Guid id)
        {
            var board = await _repo.GetByIdAsync(id);
            if (board == null) return null;
            
            return _mapper.Map<BoardReadDto>(board);
        }

        public async Task<BoardReadDto> CreateBoardAsync(BoardCreateDto dto)
        {
            var newBoard = new Board
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                ProjectId = dto.ProjectId
            };

            var createdBoard = await _repo.CreateAsync(newBoard);

            return new BoardReadDto
            {
                Id = createdBoard.Id,
                Name = createdBoard.Name,
                Columns = new List<ColumnReadDto>()
            };
        }

        public async Task<bool> DeleteBoardAsync(Guid id)
        {
            var board = await _repo.GetByIdAsync(id);
            if (board == null) return false;

            await _repo.DeleteAsync(board);
            return true;
        }
    }
}