using CryptoQuestService.Models.Dtos.Input;
using CryptoQuestService.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
                var challenges = await _cryptoQuestService.GetChallenges();
                if (!challenges.Any())
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
                var challenge = await _cryptoQuestService.GetChallenges(challengeId);
                if (challenge is null)
                    return NotFound();

                return Ok(challenge);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failure to grab challenge by id");
                return BadRequest(e);
            }
        }

        [HttpGet("{challengeId:int:required}/challenges/checkpoints")]
        public async Task<IActionResult> GetChallengeCheckpoints(int challengeId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var challengeCheckpoints = await _cryptoQuestService.GetChallengeCheckpoints(challengeId);
                if (!challengeCheckpoints.Any())
                    return NoContent();

                return Ok(challengeCheckpoints);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failure to grab challenge checkpoint by id");
                return BadRequest(e);
            }
        }

        [HttpGet("{challengeId:int:required}/challenges/checkpoints/{checkpointId:int:required}")]
        public async Task<IActionResult> GetChallengeCheckpointById(int challengeId, int checkpointId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var challengeCheckpoints = await _cryptoQuestService.GetChallengeCheckpoints(challengeId, checkpointId);
                if (!challengeCheckpoints.Any())
                    return NotFound();

                return Ok(challengeCheckpoints.First());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failure to grab challenge checkpoint by id");
                return BadRequest(e);
            }
        }

        [HttpPost("challenges/checkpoints")]
        public async Task<IActionResult> CreateChallengeCheckpoint([FromBody, Required] ChallengeCheckpointInputDto challengeCheckpointDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var challengeCheckpointId = await _cryptoQuestService.CreateChallengeCheckpoint(challengeCheckpointDto);
                return CreatedAtAction(nameof(GetChallengeCheckpointById), new { challengeId = challengeCheckpointDto.ChallengeId, checkpointId = challengeCheckpointId }, challengeCheckpointId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failure to grab challenge by id");
                return BadRequest(e);
            }
        }

        [HttpGet("challenges/checkpoints/{checkpointId:int:required}/triggers/{checkpointTriggerId:int:required}")]
        public async Task<IActionResult> GetChallengeCheckpointTrigger(int checkpointId, int checkpointTriggerId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var challengeCheckpoints = await _cryptoQuestService.GetChallengeCheckpointTriggers(checkpointId, checkpointTriggerId);

                if (!challengeCheckpoints.Any())
                    return NotFound();

                return Ok(challengeCheckpoints.First());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failure to grab challenge by id");
                return BadRequest(e);
            }
        }


        [HttpGet("challenges/checkpoints/{checkpointId:int:required}/triggers")]
        public async Task<IActionResult> GetChallengeCheckpointTriggers(int checkpointId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var challengeCheckpoints = await _cryptoQuestService.GetChallengeCheckpointTriggers(checkpointId);

                if (!challengeCheckpoints.Any())
                    return NoContent();

                return Ok(challengeCheckpoints);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failure to grab challenge by id");
                return BadRequest(e);
            }
        }

        [HttpPost("challenges/checkpoints/{checkpointId:int:required}/triggers")]
        public async Task<IActionResult> CreateChallengeCheckpointTriggers([FromBody, Required] ChallengeCheckpointTriggerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var challengeCheckpointTriggerId = await _cryptoQuestService.CreateChallengeCheckpointTrigger(dto);
                return CreatedAtRoute(nameof(GetChallengeCheckpointTrigger), new { checkpointId = dto.CheckpointId, checkpointTriggerId = challengeCheckpointTriggerId }, challengeCheckpointTriggerId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failure to grab challenge by id");
                return BadRequest(e);
            }
        }
    }
}
