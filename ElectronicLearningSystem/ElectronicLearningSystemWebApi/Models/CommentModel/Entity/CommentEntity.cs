﻿using ElectronicLearningSystemWebApi.Models.TaskModel.Entity;

namespace ElectronicLearningSystemWebApi.Models.CommentModel.Entity
{
    public class CommentEntity : EntityBase
    {
        public required string Text { get; set; }

        public TaskEntity Task { get; set; }
        public Guid TaskId { get; set; }
    }
}
