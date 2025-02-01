using ElectronicLearningSystemWebApi.Attribute;
using ElectronicLearningSystemWebApi.Helpers;
using ElectronicLearningSystemWebApi.Models.CommentModel.DTO;
using ElectronicLearningSystemWebApi.Models.CommentModel.Response;
using ElectronicLearningSystemWebApi.Models.ErrorModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Controllers
{
    /// <summary>
    /// Контроллер для работы с комментариями задач.
    /// </summary>
    /// <param name="commentHelper">Хелпер для работы с комментарями задач. </param>
    [Authorize]
    [Route("comment")]
    [ApiController]
    public class CommentController(CommentHelper commentHelper) : ControllerBase
    {
        /// <summary>
        /// Хелпер для работы с комментарями задач.
        /// </summary>
        private readonly CommentHelper _commentHelper = commentHelper
            ?? throw new ArgumentNullException(nameof(commentHelper));

        /// <summary>
        /// Получить все комментарии для задачи.
        /// </summary>
        /// <param name="id">Идентификатор задачи. </param>
        /// <response code="200">Успешный возврат комментариев для задачи. </response>
        /// <response code="500">Ошибка сервера. </response>
        [HttpGet("getcommentsbytask/{id}")]
        [ValidateModel]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommentResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetCommentsByTask(Guid id)
        {
            var comments = await _commentHelper.GetCommentsByTaskAsync(id);
            return Ok(comments);
        }

        /// <summary>
        /// Создание новой задачи.
        /// </summary>
        /// <param name="createCommentDTO">Данные о создаваемом комментарии. </param>
        /// <response code="200">Успешное создание комментария. </response>
        /// <response code="500">Ошибка сервера. </response>
        [HttpPost("create")]
        [ValidateModel]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> CreateCommentByTask([FromBody] CreateCommentDTO createCommentDTO)
        {
            await _commentHelper.CreateCommentAsync(createCommentDTO);
            return Ok();
        }
    }
}
