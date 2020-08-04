using Abp.Application.Services.Dto;
using FreeResponse.Application.SdlcSystems.Dto;
using System.Threading.Tasks;

namespace FreeResponse.Application.SdlcSystems
{
    public interface ISdlcSystemAppService
    {
        Task<ListResultDto<SdlcSystemDto>> GetAll();
        Task<bool> CheckSdlcSystemExists(int id);
    }
}

