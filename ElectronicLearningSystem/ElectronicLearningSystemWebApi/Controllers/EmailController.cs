using ElectronicLearningSystemWebApi.Attributes;
using ElectronicLearningSystemWebApi.Helpers;
using ElectronicLearningSystemWebApi.Models.EmailModel.DTO;
using ElectronicLearningSystemWebApi.Models.ErrorModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Controllers
{
    /// <summary>
    /// Контроллер для работы с Email сообщениями.
    /// </summary>
    /// <param name="emailHelper">Хелпер для работы с Email сообщениями. </param>
    [Authorize]
    [Route("email")]
    [ApiController]
    public class EmailController(EmailHelper emailHelper) : ControllerBase
    {
        /// <summary>
        /// Хелпер для работы с Email сообщениями. 
        /// </summary>
        private readonly EmailHelper _emailHelper = emailHelper 
            ?? throw new ArgumentNullException(nameof(_emailHelper));

        /// <summary>
        /// Отправка сообщения.
        /// </summary>
        /// <param name="emailResponse">Данные Email сообщения. </param>
        /// <response code="200">Успешная отправка Email. </response>
        /// <response code="500">Ошибка сервера. </response>
        [HttpPost("send")]
        [ValidateModel]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> SendEmail([FromBody] EmailSendingDTO emailResponse)
        {
            await _emailHelper.SendEmailAsync(emailResponse);
            return Ok();
        }
    }
}
