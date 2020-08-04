using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System;

namespace FreeResponse.Application.SdlcSystems.Dto
{
    public class SdlcSystemDto : EntityDto, IHasCreationTime
    {
        public string BaseUrl { get; set; }
        public string Description { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreationTime { get; set; }
    }
}

