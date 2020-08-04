using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using FreeResponse.Core.SdlcSystems;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreeResponse.Core.Projects
{
    [Table("project")]
    public class Project : IIdentifier, IHasCreationTime, IHasModificationTime
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column("external_id")]
        public string ExternalId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Required]
        [Column("last_modified_date")]
        public DateTime? LastModificationTime { get; set; }
        [Required]
        [Column("created_date")]
        public DateTime CreationTime { get; set; }
        public virtual SdlcSystem SdlcSystem { get; set; }

        [ForeignKey("SdlcSystemId")]
        [Range(1, int.MaxValue)]
        public int SdlcSystemId { get; set; }

        public Project()
        {
            CreationTime = Clock.Now;
            LastModificationTime = Clock.Now;
        }
    }
}

