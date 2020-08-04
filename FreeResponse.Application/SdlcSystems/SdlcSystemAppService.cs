using Abp.Application.Services.Dto;
using FreeResponse.Application.SdlcSystems.Dto;
using FreeResponse.Core.SdlcSystems;
using FreeResponse.EntityFrameworkCore.Repositories;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using System;
using log4net;

namespace FreeResponse.Application.SdlcSystems
{
    public class SdlcSystemAppService : ISdlcSystemAppService
    {
        private readonly IRepository<SdlcSystem> _sdlcSystemRepository;
        private readonly IMapper _mapper;
        static readonly ILog _log4net = LogManager.GetLogger(typeof(SdlcSystemAppService));

        public SdlcSystemAppService(IRepository<SdlcSystem> sdlcSystemRepository, IMapper mapper)
        {
            _sdlcSystemRepository = sdlcSystemRepository;
            _mapper = mapper;
        }
        public async Task<ListResultDto<SdlcSystemDto>> GetAll()
        {
            try
            {
                var sdlcSystems = (await _sdlcSystemRepository.GetAll()).ToList();

                return new ListResultDto<SdlcSystemDto>(
                    _mapper.Map<List<SdlcSystemDto>>(sdlcSystems)
                );
            }
            catch (Exception ex)
            {
                _log4net.Info(ex.Message, ex);
                throw;
            }
        }

        public async Task<bool> CheckSdlcSystemExists(int id)
        {
            try
            {
                return (await _sdlcSystemRepository.Get(id)) != null || id == 0;
            }
            catch (Exception ex)
            {
                _log4net.Info(ex.Message, ex);
                throw;
            }
        }
    }
}

