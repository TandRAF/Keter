using AutoMapper;
using server.Dtos.ColumnDto;
using server.Interfaces;
using server.Models;

namespace server.Services
{
    public class ColumnService : IColumnService
    {
        private readonly IColumnRepository _repo;
        private readonly IMapper _mapper;

        public ColumnService(IColumnRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ColumnReadDto> CreateColumnAsync(ColumnCreateDto createDto)
        {
            var columnModel = _mapper.Map<BoardColumn>(createDto);
            var createdColumn = await _repo.CreateAsync(columnModel);
            return _mapper.Map<ColumnReadDto>(createdColumn);
        }

        public async Task<bool> UpdateColumnAsync(Guid id, ColumnUpdateDto updateDto)
        {
            var column = await _repo.GetByIdAsync(id);
            if (column == null) return false;
            column.Title = updateDto.Title;
            column.Order = updateDto.Order;

            await _repo.UpdateAsync(column);
            return true;
        }

        public async Task<bool> DeleteColumnAsync(Guid id)
        {
            var column = await _repo.GetByIdAsync(id);
            if (column == null) return false;

            await _repo.DeleteAsync(column);
            return true;
        }
    }
}