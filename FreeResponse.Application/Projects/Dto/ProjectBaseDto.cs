using System.ComponentModel.DataAnnotations;

namespace FreeResponse.Application.Projects.Dto
{
    public class ProjectBaseDto
    {
        [Required]
        public string ExternalId { get; set; }
        public string Name { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int SdlcSystemId { get; set; }
    }
}

