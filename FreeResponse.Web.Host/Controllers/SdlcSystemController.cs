using System;
using System.Threading.Tasks;
using FreeResponse.Application.SdlcSystems;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace FreeResponse.Web.Host.Controllers
{
    [Route("api/v2/SdlcSystem")]
    public class SdlcSystemController : Controller
    {
        private readonly ISdlcSystemAppService _sdlcSystemAppService;
        static readonly ILog _log4net = LogManager.GetLogger(typeof(ProjectController));

        public SdlcSystemController(ISdlcSystemAppService sdlcSystemAppService)
        {
            _sdlcSystemAppService = sdlcSystemAppService;
        }

        /// <summary>
        /// Get all SdlcSystems.
        /// </summary>

        /// <returns code="200">A list of Systems</returns>

        [HttpGet("SdlcSystem")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var dtOs = await _sdlcSystemAppService.GetAll();
                return Ok(dtOs);
            }
            catch (Exception ex)
            {
                _log4net.Info(ex.Message, ex);
                return StatusCode(500);
            }
        }
    }
}