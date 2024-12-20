using AutoMapper;
using ElectronicLearningSystemCore.Extensions;
using ElectronicLearningSystemWebApi.Enums;
using ElectronicLearningSystemWebApi.Helpers;
using ElectronicLearningSystemWebApi.Models.CommentModel;
using ElectronicLearningSystemWebApi.Models.CommentModel.DTO;
using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Repositories.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Controllers
{
    [Route("comment")]
    [ApiController]
    public class CommentController(IRepository<CommentEntity> commentRepository,
        ILogger<CommentController> logger, IMapper mappingProfile) : ControllerBase
    {
        private readonly IRepository<CommentEntity> _commentRepository = commentRepository 
            ?? throw new ArgumentNullException(nameof(commentRepository));

        private readonly ILogger<CommentController> _logger = logger 
            ?? throw new ArgumentNullException(nameof(logger));

        private readonly IMapper _mappingProfile = mappingProfile
            ?? throw new ArgumentNullException(nameof(mappingProfile));

        [HttpGet("getcommentsbytask/{id}")]
        public async Task<IActionResult> GetCommentsByTask(Guid id)
        {
            id.ThrowIsDefault();

            try
            {
                return Ok(await _commentRepository.GetRecordsByQueryAsync(x => x.TaskId == id));
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId((int)EventLoggerEnum.DataBaseException), message: ex.ToString());
                return BadRequest();
            }
        }

        [HttpPost("createcomment")]
        public async Task<IActionResult> CreateCommentByTask([FromBody] CreateCommentDTO createComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var comment = _mappingProfile.Map<CommentEntity>(createComment);
                if (comment is null)
                {
                    _logger.LogError(new EventId((int)EventLoggerEnum.InvalidMapEntity), message: $"Invalid map comment {createComment.TaskId}");
                    return BadRequest();
                }

                await _commentRepository.AddRecordAsync(comment);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId((int)EventLoggerEnum.DataBaseException), message: ex.ToString());
                return BadRequest();
            }
        }
    }
}
