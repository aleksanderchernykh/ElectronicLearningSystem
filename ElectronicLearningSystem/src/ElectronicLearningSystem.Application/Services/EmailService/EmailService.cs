using AutoMapper;
using ElectronicLearningSystem.Common.Helpers.EmailSendingHelper;
using ElectronicLearningSystemKafka.Common.Models;
using ElectronicLearningSystemWebApi.Models.EmailModel.DTO;

namespace ElectronicLearningSystem.Application.Services.EmailService
{
    /// <summary>
    /// Хелпер для работы с Email сообщениями.
    /// </summary>
    /// <param name="emailSendingService">Хелпер для работы с отправкой сообщения. </param>
    /// <param name="mapper">Маппер. </param>
    public class EmailService(IEmailSendingHelper emailSendingService,
        IMapper mapper) : IEmailService
    {
        /// <summary>
        /// Хелпер для работы с отправкой сообщения.
        /// </summary>
        private readonly IEmailSendingHelper _emailSendingService = emailSendingService;

        /// <summary>
        /// Маппер.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Отправка сообщения.
        /// </summary>
        /// <param name="emailResponse">Данные Email сообщения.</param>
        public async Task SendEmailAsync(EmailSendingDTO emailResponse)
        {
            var email = _mapper.Map<Email>(emailResponse);
            await _emailSendingService.SendEmailAsync(email);
        }
    }
}
