using Abp.Application.Services.Dto;
using FreeResponse.Application.Projects.Dto;
using System.Threading.Tasks;

namespace FreeResponse.Application.Projects
{
    public interface IProjectAppService
    {
        Task<ListResultDto<ProjectDto>> GetAllAsync();
        Task<ProjectDto> GetProject(int projectId);
        ProjectDto CreateProject(ProjectBaseDto projectDto);
        Task<bool?> UpdateProject(int id, ProjectBaseDto project);
        Task<bool> CheckForConflicting(int sdlcSystemId, string externalId);
        Task<bool> CheckForConflictingSystem(int sdlcSystemId, int id);
        Task<bool> CheckForConflictingExternalId(string externalId, int id);

    }
}

