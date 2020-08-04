using AutoMapper;
using FreeResponse.Application.Projects.Dto;
using FreeResponse.Application.SdlcSystems.Dto;
using FreeResponse.Core.Projects;
using FreeResponse.Core.SdlcSystems;

namespace FreeResponse.Application.MapperConfiguration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SdlcSystem, SdlcSystemDto>();
            CreateMap<SdlcSystemDto, SdlcSystem>();

            CreateMap<Project, ProjectBaseDto>()
                .ForMember(opts => opts.SdlcSystemId, opt => opt.Condition(source => source.SdlcSystemId > 0))
            .ForAllOtherMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ProjectBaseDto, Project>()
                .ForMember(opts => opts.SdlcSystemId, opt => opt.Condition(source => source.SdlcSystemId > 0))
                .ForAllOtherMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();
        }
    }
}

