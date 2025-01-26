import { Component, inject, Input } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommentForm } from '../../interfaces/forms/comment-form';
import { CommentService } from '../../services/comment.service';

@Component({
  selector: 'app-new-comment',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './new-comment.component.html',
  styleUrl: './new-comment.component.scss'
})
export class NewCommentComponent {
  @Input() taskid!: string;
  commentService = inject(CommentService);
  commentForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.commentForm = this.fb.group({
      comment: ['', [Validators.required, Validators.minLength(5)]],
    });
  }
  
  onCreateComment($event: Event) {
    const formData: CommentForm = { 
      taskId: this.taskid, 
      text: this.commentForm.value.comment 
    };
  
    // Подписка на результат запроса
    this.commentService.createcomment(formData).subscribe(
      response => {
        console.log('Comment created successfully', response);
      },
      error => {
        console.error('Error creating comment', error);
      }
    );
  
    console.log("comment");
  }
}
