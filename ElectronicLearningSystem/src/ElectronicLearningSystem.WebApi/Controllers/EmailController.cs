using ElectronicLearningSystem.Application.Models.ErrorModel;
using ElectronicLearningSystem.Application.Services.EmailService;
using ElectronicLearningSystem.WebApi.Attributes;
using ElectronicLearningSystemWebApi.Models.EmailModel.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystem.WebApi.Controllers
{
    /// <summary>
    /// Контроллер для работы с Email сообщениями.
    /// </summary>
    /// <param name="emailService">Хелпер для работы с Email сообщениями. </param>
    [Authorize]
    [Route("email")]
    [ApiController]
    public class EmailController(IEmailService emailService)
        : ControllerBase
    {
        /// <summary>
        /// Хелпер для работы с Email сообщениями. 
        /// </summary>
        private readonly IEmailService _emailService = emailService
            ?? throw new ArgumentNullException(nameof(_emailService));

        /// <summary>
        /// Отправка сообщения.
        /// </summary>
        /// <param name="emailDTO">Данные Email сообщения. </param>
        /// <response code="201">Успешное создание Email. </response>
        /// <response code="500">Ошибка сервера. </response>
        [HttpPost("create")]
        [ValidateModel]
        [Produces("application/json")]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> SendEmail([FromBody] EmailSendingDTO emailDTO)
        {
            await _emailService.SendEmailAsync(emailDTO);
            return Created();
        }
    }
}
