using server.Dtos.BoardDto;
using server.Dtos.ColumnDto;
using server.Dtos.TaskDto;
using server.Dtos.TagDto;
using server.Repositories;
using server.Interfaces;

namespace server.Services
{

    public class BoardService : IBoardService
    {
        private readonly IBoardRepository _boardRepo;

        public BoardService(IBoardRepository boardRepo)
        {
            _boardRepo = boardRepo;
        }

        public async Task<BoardReadDto?> GetBoardDataAsync(Guid boardId)
        {
            var board = await _boardRepo.GetBoardWithDetailsAsync(boardId);
            if (board == null) return null;

            // Mapare manuală (dacă nu folosești AutoMapper)
            return new BoardReadDto
            {
                Id = board.Id,
                Name = board.Name,
                ProjectId = board.ProjectId,
                Columns = board.Columns.Select(c => new ColumnReadDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Order = c.Order,
                    Tasks = c.Tasks.Select(t => new TaskReadDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        ColumnId = t.ColumnId,
                        Order = t.Order,
                        Status = c.Title, // Aici setăm Status-ul cu numele coloanei!
                        AssignedUserId = t.AssignedUserId,
                        Tags = t.Tags.Select(tag => new TagReadDto
                        {
                            Id = tag.Id,
                            Name = tag.Name,
                            ColorHex = tag.ColorHex
                        }).ToList()
                    }).ToList()
                }).ToList()
            };
        }
    }
}