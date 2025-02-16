using ElectronicLearningSystem.Application.Models.CommentModel.DTO;
using ElectronicLearningSystem.Application.Models.CommentModel.Response;
using ElectronicLearningSystem.Application.Models.ErrorModel;
using ElectronicLearningSystem.Application.Services.CommentService;
using ElectronicLearningSystem.WebApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystem.WebApi.Controllers
{
    /// <summary>
    /// Контроллер для работы с комментариями задач.
    /// </summary>
    /// <param name="commentService">Хелпер для работы с комментарями задач. </param>
    [Authorize]
    [Route("comment")]
    [ApiController]
    public class CommentController(ICommentService commentService)
        : ControllerBase
    {
        /// <summary>
        /// Хелпер для работы с комментарями задач.
        /// </summary>
        private readonly ICommentService _commentService = commentService
            ?? throw new ArgumentNullException(nameof(commentService));

        /// <summary>
        /// Получить все комментарии для задачи.
        /// </summary>
        /// <param name="id">Идентификатор задачи. </param>
        /// <response code="200">Успешный возврат комментариев для задачи. </response>
        /// <response code="500">Ошибка сервера. </response>
        [HttpGet("getbytask/{id}")]
        [ValidateModel]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommentResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetCommentsByTask(Guid id)
        {
            var comments = await _commentService.GetCommentsByTaskAsync(id);
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
            await _commentService.CreateCommentAsync(createCommentDTO);
            return Created();
        }
    }
}
