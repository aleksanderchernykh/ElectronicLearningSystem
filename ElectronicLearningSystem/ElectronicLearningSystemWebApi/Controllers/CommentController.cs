using ElectronicLearningSystemCore.Extensions;
using ElectronicLearningSystemWebApi.Enums;
using ElectronicLearningSystemWebApi.Helpers;
using ElectronicLearningSystemWebApi.Models.CommentModel;
using ElectronicLearningSystemWebApi.Models.CommentModel.DTO;
using ElectronicLearningSystemWebApi.Repositories.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Controllers
{
    [Authorize]
    [Route("comment")]
    [ApiController]
    public class CommentController(IRepository<CommentEntity> commentRepository,
        ILogger<CommentController> logger, CommentHelper commentHelper) : ControllerBase
    {
        private readonly IRepository<CommentEntity> _commentRepository = commentRepository 
            ?? throw new ArgumentNullException(nameof(commentRepository));

        private readonly ILogger<CommentController> _logger = logger 
            ?? throw new ArgumentNullException(nameof(logger));

        private readonly CommentHelper _commentHelper = commentHelper
            ?? throw new ArgumentNullException(nameof(commentHelper));

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
                _logger.LogError(new EventId((int)EventLoggerEnum.DataBaseException),
                    message: ex.ToString());
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
                var comment = commentHelper.GetCommentByDTO(createComment);
                if (comment is null)
                {
                    _logger.LogError(new EventId((int)EventLoggerEnum.InvalidMapEntityException),
                        message: $"Invalid map comment {createComment.TaskId}");
                    return BadRequest();
                }

                await _commentRepository.AddRecordAsync(comment);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId((int)EventLoggerEnum.DataBaseException),
                    message: ex.ToString());
                return BadRequest();
            }
        }
    }
}
