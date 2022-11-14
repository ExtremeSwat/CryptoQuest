using CryptoQuestService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoQuestService.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ChallengesController : ControllerBase
    {
        private readonly CryptoQuestOperationsService _cryptoQuestService;
        private readonly ILogger<ChallengesController> _logger;

        public ChallengesController(CryptoQuestOperationsService cryptoQuestService, ILogger<ChallengesController> logger)
        {
            _cryptoQuestService = cryptoQuestService;
            _logger = logger;
        }

        /// <summary>
        /// Returns all of <see cref="Challenges"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GrabAllChallenges()
        {
            try
            {
                var challenges = await _cryptoQuestService.GrabCurrentChallenges();
                if(!challenges.Any())
                    return NoContent();

                return Ok(challenges);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failure to grab challenges");
                return BadRequest();
            }
        }

        [HttpGet("{challengeId:int:required}")]
        public async Task<IActionResult> GrabChallengeById(int challengeId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var challenge = await _cryptoQuestService.GrabChallengeById(challengeId);
                if (challenge is null)
                    return NotFound();

                return Ok(challenge);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failure to grab challenge by id");
                return BadRequest();
            }
        }
    }
}
