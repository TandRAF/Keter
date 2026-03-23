// Services/ProjectService.cs
using AutoMapper;
using server.Models;
using server.Dtos;
using server.Interfaces;
using server.Dtos.ProjectDto;
using Microsoft.AspNetCore.Identity;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repo;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProjectService(IProjectRepository repo, IMapper mapper,UserManager<ApplicationUser> userManager)
    {
        _repo = repo;
        _userManager = userManager;
        _mapper = mapper;
    }

   public async Task<IEnumerable<ProjectReadDto>> GetAllEntitiesAsync(string userId) 
   {
    var entities = await _repo.GetAllByUserIdAsync(userId);
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

   public async Task<bool> UpdateEntityAsync(Guid id, ProjectUpdateDto updateDto, string userId)
    {
        var existingEntity = await _repo.GetByIdAsync(id);
        if (existingEntity == null || existingEntity.OwnerId != userId) 
        {
            return false;
        }
        
        existingEntity.Name = updateDto.Name;
        existingEntity.Description = updateDto.Description;
        await _repo.UpdateAsync(existingEntity);
        return true;
    }

    public async Task<bool> DeleteEntityAsync(Guid id, string userId)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null || entity.OwnerId != userId) 
        {
            return false;
        }
        
        await _repo.DeleteAsync(entity);
        return true;
    }
   public async Task<bool> AddMemberAsync(Guid projectId, string username, string requesterId)
    {
        var project = await _repo.GetByIdAsync(projectId);
        if (project == null || project.OwnerId != requesterId) return false;

        var userToAdd = await _userManager.FindByNameAsync(username);
        
        if (userToAdd == null || project.Members.Any(m => m.Id == userToAdd.Id)) return false; 

        project.Members.Add(userToAdd);
        return await _repo.SaveChangesAsync(); 
    }

    public async Task<bool> RemoveMemberAsync(Guid projectId, string memberId, string requesterId)
    {
        var project = await _repo.GetByIdAsync(projectId);
        if (project == null) return false;
        if (project.OwnerId != requesterId && memberId != requesterId) return false;
        var memberToRemove = project.Members.FirstOrDefault(m => m.Id == memberId);
        if (memberToRemove == null) return false;
        project.Members.Remove(memberToRemove);
        return await _repo.SaveChangesAsync();
}
    
}