using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Pkcs11Test.CustomClasses;

namespace Pkcs11Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController(IHsmAdapter adapter) : ControllerBase
    {
        [HttpGet("generateKeyPair", Name = "GenerateKeyPair")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<ActionResult<List<ApplicationModel>>> GetAllApplications()
        {
            try
            {
                using var hsm = new Hsm(adapter);
                hsm.Initialize();
                var keys = hsm.GenerateKeyPair();

                return Ok(keys.Item1);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
