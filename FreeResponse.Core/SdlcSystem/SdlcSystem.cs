using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using FreeResponse.Core.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreeResponse.Core.SdlcSystems
{
    [Table("sdlc_system")]
    public class SdlcSystem : IIdentifier, IHasCreationTime
    {
        public const int MaxDescriptionLength = 64 * 1024; //64KB

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Url]
        [Column("base_url")]
        public string BaseUrl { get; set; }
        [StringLength(MaxDescriptionLength)]
        public string Description { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreationTime { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}

