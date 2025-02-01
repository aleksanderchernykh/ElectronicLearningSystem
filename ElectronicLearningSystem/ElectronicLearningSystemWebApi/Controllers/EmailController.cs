using AutoMapper;
using ElectronicLearningSystemKafka.Common.Models;
using ElectronicLearningSystemWebApi.Enums;
using ElectronicLearningSystemWebApi.Helpers;
using ElectronicLearningSystemWebApi.Models.EmailModel.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Controllers
{
    [Authorize]
    [Route("email")]
    [ApiController]
    public class EmailController(EmailSendingHelper emailSendingService, 
        IMapper mapper,
        ILogger<EmailController> logger) : ControllerBase
    {
        private readonly EmailSendingHelper _emailSendingService = emailSendingService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<EmailController> _logger = logger;

        /// <summary>
        /// Отправка email сообщения в топик.
        /// </summary>
        [HttpPost("sendemail")]
        public async Task<IActionResult> SendEmail([FromBody] EmailSendingDTO emailResponse)
        {
            try
            {
                var email = _mapper.Map<Email>(emailResponse);
                if (email is null)
                {
                    _logger.LogError((int)EventLoggerEnum.EmailSendingException, "Email not valid");
                    return BadRequest("Email not valid");
                }

                await _emailSendingService.SendEmailAsync(email);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError((int)EventLoggerEnum.EmailSendingException, message: ex.ToString());
                return BadRequest(ex.Message);
            }
        }
    }
}
