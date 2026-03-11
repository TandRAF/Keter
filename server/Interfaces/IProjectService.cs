using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.ProjectDto;

namespace server.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectReadDto>> GetAllEntitiesAsync(string userId);
        Task<ProjectReadDto?> GetEntityByIdAsync(Guid id);
        Task<ProjectReadDto> CreateEntityAsync(ProjectCreateDto entityCreateDto);
        Task<bool> UpdateEntityAsync(Guid id, ProjectUpdateDto updateDto,string userId);
        Task<bool> DeleteEntityAsync(Guid id,string userId);
    }
}