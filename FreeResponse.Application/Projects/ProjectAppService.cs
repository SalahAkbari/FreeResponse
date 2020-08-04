using Abp.Application.Services.Dto;
using FreeResponse.EntityFrameworkCore.Repositories;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FreeResponse.Core.Projects;
using FreeResponse.Application.Projects.Dto;
using System;
using Abp.Timing;
using log4net;
using FreeResponse.Core.SdlcSystems;

namespace FreeResponse.Application.Projects
{
    public class ProjectAppService : IProjectAppService
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IMapper _mapper;
        static readonly ILog _log4net = LogManager.GetLogger(typeof(ProjectAppService));

        public ProjectAppService(IRepository<Project> projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }
        public async Task<ListResultDto<ProjectDto>> GetAllAsync()
        {
            try
            {
                var projects = (await _projectRepository.GetAll()).ToList();

                return new ListResultDto<ProjectDto>(
                    _mapper.Map<List<ProjectDto>>(projects)
                );
            }
            catch (Exception ex)
            {
                _log4net.Info(ex.Message, ex);
                throw;
            }
        }

        public async Task<ProjectDto> GetProject(int projectId)
        {
            try
            {
                var project = (await _projectRepository.Get(projectId));

                return _mapper.Map<ProjectDto>(project);
            }
            catch (Exception ex)
            {
                _log4net.Info(ex.Message, ex);
                throw;
            }
        }

        public ProjectDto CreateProject(ProjectBaseDto projectDto)
        {
            try
            {
                var itemToCreate = _mapper.Map<Project>(projectDto);
                _projectRepository.Add(itemToCreate);
                _projectRepository.Save();
                var createdDto = _mapper.Map<ProjectDto>(itemToCreate);
                return createdDto;
            }
            catch (Exception ex)
            {
                _log4net.Info(ex.Message, ex);
                throw;
            }
        }

        public async Task <bool?> UpdateProject(int id, ProjectBaseDto projectDto)
        {
            try
            {
                var entityToUpdate = await _projectRepository.Get(id);
                if (entityToUpdate == null) return false;
                entityToUpdate.LastModificationTime = Clock.Now;
                _mapper.Map(projectDto, entityToUpdate);
                if (!_projectRepository.Save()) return null;
                return true;
            }
            catch (Exception ex)
            {
                _log4net.Info(ex.Message, ex);
                throw;
            }
        }

        public async Task<bool> CheckForConflicting(int sdlcSystemId, string externalId)
        {
            try
            {
                return (await _projectRepository.GetAll())
                    .Any(c => c.SdlcSystemId.Equals(sdlcSystemId)
                    && c.ExternalId.Equals(externalId));
            }
            catch (Exception ex)
            {
                _log4net.Info(ex.Message, ex);
                throw;
            }
        }

        public async Task<bool> CheckForConflictingSystem(int sdlcSystemId, int id)
        {
            try
            {
                return (await _projectRepository.GetAll())
                    .Any(c => c.SdlcSystemId.Equals(sdlcSystemId)
                    && c.Id.Equals(id));
            }
            catch (Exception ex)
            {
                _log4net.Info(ex.Message, ex);
                throw;
            }
        }

        public async Task<bool> CheckForConflictingExternalId(string externalId, int id)
        {
            try
            {
                return (await _projectRepository.GetAll())
                    .Any(c => c.ExternalId.Equals(externalId)
                    && c.Id.Equals(id));
            }
            catch (Exception ex)
            {
                _log4net.Info(ex.Message, ex);
                throw;
            }
        }
    }
}

