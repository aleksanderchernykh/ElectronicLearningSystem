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
    const formData: CommentForm = { task: this.taskid, comment: this.commentForm.value.comment };
    this.commentService.createcomment(formData);
    console.log("comment");
  }
}
