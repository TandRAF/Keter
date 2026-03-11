using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.ProjectDto;
using server.Models;
using AutoMapper;
using server.Dtos.BoardDto;
using server.Dtos.TaskDto;

namespace server.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // --- MAPPING PENTRU PROIECTE ---
            // Read: Din entitate în DTO
            CreateMap<Project, ProjectReadDto>();
            CreateMap<ProjectCreateDto, Project>();
            CreateMap<ProjectUpdateDto, Project>();


            // --- MAPPING PENTRU BOARDS ---
            CreateMap<Board, BoardReadDto>();
            CreateMap<BoardCreateDto, Board>();
            CreateMap<BoardUpdateDto, Board>();


            // --- MAPPING PENTRU TASKS ---
            // Folosim ProjectTask pentru a evita ambiguitatea cu System.Threading.Tasks
            CreateMap<ProjectTask, TaskReadDto>();
            CreateMap<TaskCreateDto, ProjectTask>();
            CreateMap<TaskUpdateDto, ProjectTask>();
        }
    }
}