using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.BoardDto;

namespace server.Interfaces
{
    public interface IBoardService
    {
        Task<IEnumerable<BoardReadDto>> GetBoardsByProjectAsync(Guid projectId);
        Task<BoardReadDto?> GetBoardByIdAsync(Guid id);
        Task<BoardReadDto> CreateBoardAsync(BoardCreateDto dto);
        Task<bool> DeleteBoardAsync(Guid id);
    }
}