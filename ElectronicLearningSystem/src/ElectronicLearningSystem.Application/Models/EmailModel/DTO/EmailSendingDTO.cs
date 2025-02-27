﻿namespace ElectronicLearningSystemWebApi.Models.EmailModel.DTO
{
    /// <summary>
    /// Email сообщение.
    /// </summary>
    public class EmailSendingDTO
    {
        /// <summary>
        /// Тема.
        /// </summary>
        public required string Subject { get; set; }

        /// <summary>
        /// Текст.
        /// </summary>
        public required string Text { get; set; }

        /// <summary>
        /// Получатели.
        /// </summary>
        public required string[] Recipients { get; set; }

        /// <summary>
        /// Файлы.
        /// </summary>
        public string[]? Files { get; set; }
    }
}
