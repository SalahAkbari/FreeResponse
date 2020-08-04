using System;
using System.Threading.Tasks;
using FreeResponse.Application.Projects;
using FreeResponse.Application.Projects.Dto;
using FreeResponse.Application.SdlcSystems;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeResponse.Web.Host.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectAppService _projectAppService;
        private readonly ISdlcSystemAppService _sdlcSystemAppService;
        static readonly ILog _log4net = LogManager.GetLogger(typeof(ProjectController));


        public ProjectController(IProjectAppService projectAppService,
            ISdlcSystemAppService sdlcSystemAppService)
        {
            _projectAppService = projectAppService;
            _sdlcSystemAppService = sdlcSystemAppService;
        }

        /// <summary>
        /// Get all Projects.
        /// </summary>

        /// <returns code="200">a list of Project</returns>

        [HttpGet("api/v2/projects")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var items = await _projectAppService.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                _log4net.Info(ex.Message, ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Get a Project.
        /// </summary>

        /// <returns code="200">a Project</returns>
        /// <response code="400">In case Illegal variable is passed</response> 
        /// <response code="404">If the ProjectDto based on the projectId could not be found</response> 

        [HttpGet("api/v2/projects/{id}", Name = "GetProject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest();

                var item = await _projectAppService.GetProject(id);
                if (item == null)
                    return NotFound();//404 Not Found (Client Error Status Code)

                return Ok(item);
            }
            catch (Exception ex)
            {
                _log4net.Info(ex.Message, ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Creates a new Project.
        /// </summary>

        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the dto is null or the ModelState is invalid</response> 
        /// <response code="404">If the SdlcSystem based on the SdlcSystemId could not be found</response> 
        /// <response code="409">In case Payload Containing Conflicting System / External ID</response> 

        [HttpPost("api/v2/projects")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateProject([FromBody]ProjectBaseDto dto)
        {
            try
            {
                if (dto == null) return BadRequest();
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var exists = await _sdlcSystemAppService.CheckSdlcSystemExists(dto.SdlcSystemId);
                if (!exists) return NotFound();

                var conflict = await _projectAppService.CheckForConflicting(dto.SdlcSystemId, dto.ExternalId);
                if (conflict) return Conflict();

                var result = _projectAppService.CreateProject(dto);
                return result == null ? StatusCode(500, "A problem occurred while handling your request.")
                    : CreatedAtRoute("GetProject", new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _log4net.Info(ex.Message, ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Update a Project.
        /// </summary>

        /// <response code="200"></response>
        /// <response code="400">If the dto is null or Illegal variable is passed</response> 
        /// <response code="404">If the SdlcSystem based on the SdlcSystemId could not be found, or invalid variable is passed</response> 
        /// <response code="409">In case Payload Containing Conflicts</response> 

        [HttpPatch("api/v2/projects/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateProject(int id, [FromBody]ProjectBaseDto dto)
        {
            try
            {
                if (dto == null || id == 0) return BadRequest();

                var exists = await _sdlcSystemAppService.CheckSdlcSystemExists(dto.SdlcSystemId);
                if (!exists) return NotFound();

                var conflictSystem = await _projectAppService.CheckForConflictingSystem(dto.SdlcSystemId, id);
                if (conflictSystem && string.IsNullOrWhiteSpace(dto.ExternalId)
                    && string.IsNullOrWhiteSpace(dto.Name))
                    return Conflict();

                var conflictExternalId = await _projectAppService.CheckForConflictingExternalId(dto.ExternalId, id);
                if (conflictExternalId && dto.SdlcSystemId == 0
                    && string.IsNullOrWhiteSpace(dto.Name))
                    return Conflict();

                var result = await _projectAppService.UpdateProject(id, dto);

                if (!result.Value)
                    return NotFound();

                if (result == null)
                    return StatusCode(500, "A problem occurred while handling your request.");

                return Ok();//We have also the option to return NoContent(); instead, for update usually, successfull request that shouldn't return anything
            }
            catch (Exception ex)
            {
                _log4net.Info(ex.Message, ex);
                return StatusCode(500);
            }
        }
    }
}