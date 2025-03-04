﻿using ElectronicLearningSystemWebApi.Models.EmailModel.DTO;

namespace ElectronicLearningSystem.Application.Services.EmailService
{
    /// <summary>
    /// Интерфейс сервиса для работы с Email сообщениями.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Отправка сообщения.
        /// </summary>
        /// <param name="emailResponse">Данные Email сообщения.</param>
        Task SendEmailAsync(EmailSendingDTO emailResponse);
    }
}
