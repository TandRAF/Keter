using server.Dtos.ProjectDto;
using server.Models;
using AutoMapper;
using server.Dtos.BoardDto;
using server.Dtos.TaskDto;
using server.Dtos.ColumnDto;
using server.Dtos.TagDto;
using server.Dtos.UserDto;

namespace server.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserPayloadDto>();
            // MAPPING PENTRU PROIECTE
            CreateMap<Project, ProjectReadDto>();
            CreateMap<ProjectCreateDto, Project>();
            CreateMap<ProjectUpdateDto, Project>();

            // MAPPING PENTRU BOARDS
            CreateMap<Board, BoardReadDto>();
            CreateMap<BoardCreateDto, Board>();
            CreateMap<BoardUpdateDto, Board>();

            // ADAUGĂ ASTA PENTRU COLOANE (BOARD COLUMNS)
            CreateMap<BoardColumn, ColumnReadDto>();
            CreateMap<ColumnCreateDto, BoardColumn>();
            CreateMap<ColumnUpdateDto, BoardColumn>();

            // MAPPING PENTRU TASKS
            CreateMap<ProjectTask, TaskReadDto>();
            CreateMap<TaskCreateDto, ProjectTask>();
            CreateMap<TaskUpdateDto, ProjectTask>();
            
            // MAPPING TAGS
            CreateMap<Tag, TagReadDto>(); 
            CreateMap<TagCreateDto, Tag>();

            // MAPPING Columns
            CreateMap<ColumnCreateDto, BoardColumn>();
            CreateMap<BoardColumn, ColumnReadDto>();
        }
    }
}