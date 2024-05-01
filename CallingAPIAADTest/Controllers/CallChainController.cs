using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CallingAPIAADTest.Controllers
{
    [ApiController]
    [Route("api1/call-chain")]
    public class CallChainController : ControllerBase
    {
        private readonly ILogger<CallChainController> _logger;
        private readonly IDownstreamAPIClient _client;

        public CallChainController(ILogger<CallChainController> logger, IDownstreamAPIClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet(Name = "GetCallChain")]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("About to call downstream API");
                CallingAPIResponse response = await _client.CallDownstreamAPI();
                return Ok(response);
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
