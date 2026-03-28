using server.Dtos.ColumnDto;

namespace server.Interfaces
{
    public interface IColumnService
    {
        Task<ColumnReadDto> CreateColumnAsync(ColumnCreateDto createDto);
        Task<bool> UpdateColumnAsync(Guid id, ColumnUpdateDto updateDto);
        Task<bool> DeleteColumnAsync(Guid id);
    }
}