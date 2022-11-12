using Microsoft.AspNetCore.Mvc;

namespace CryptoQuestService.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ChallengesController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateChallenge()
        {
            return Ok();
        }
    }
}
