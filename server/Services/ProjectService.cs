// Services/ProjectService.cs
using AutoMapper;
using server.Models;
using server.Dtos;
using server.Interfaces;
using server.Dtos.ProjectDto;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repo;
    private readonly IMapper _mapper;

    public ProjectService(IProjectRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProjectReadDto>> GetAllEntitiesAsync()
    {
        var entities = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<ProjectReadDto>>(entities);
    }

    public async Task<ProjectReadDto?> GetEntityByIdAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return null;
        return _mapper.Map<ProjectReadDto>(entity);
    }

    public async Task<ProjectReadDto> CreateEntityAsync(ProjectCreateDto entityCreateDto)
    {
        var entityModel = _mapper.Map<Project>(entityCreateDto);
        await _repo.CreateAsync(entityModel);
        return _mapper.Map<ProjectReadDto>(entityModel);
    }

    public async Task<bool> UpdateEntityAsync(Guid id, ProjectUpdateDto updateDto)
    {
        var existingEntity = await _repo.GetByIdAsync(id);
        if (existingEntity == null) return false;
        
        _mapper.Map(updateDto, existingEntity);
        await _repo.UpdateAsync(existingEntity);
        return true;
    }

    public async Task<bool> DeleteEntityAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;
        
        await _repo.DeleteAsync(entity);
        return true;
    }
}