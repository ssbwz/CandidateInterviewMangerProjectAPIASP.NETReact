using AppointmentAPI.DecoderService;
using LogicLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentAPI.Controllers
{
    [Route("/test")]
    [ApiController]
    public class TestControllerForAuthentication : Controller
    {
        public IDecoderService DecoderService { get; set; }
        public TestControllerForAuthentication(IDecoderService decoderService)
        {
            DecoderService = decoderService;
        }

        [HttpGet]
        public IActionResult HelloAuthorization()
        {
            try
            {
                DecoderService.Authorize(new List<string> { "Recruiter" });
                DecryptedUser claimData = DecoderService.GetCredentials();
                /*string currentUser = DecodeToken();*/
                return Ok($"Hello there {claimData.Role}!");
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized(uae.Message);
            }
        }
    }
}
