import { Component, inject} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Task } from '../../interfaces/task';
import { TaskService } from '../../services/task.service';
import { CommentComponent } from "../../common-ui/comment/comment.component";
import { CommentService } from '../../services/comment.service';
import { Comment } from '../../interfaces/comment';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommentForm } from '../../interfaces/forms/comment-form';
import { NotificationService } from '../../services/notification.service';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-task-page',
  standalone: true,
  imports: [CommentComponent, ReactiveFormsModule],
  templateUrl: './task-page.component.html',
  styleUrl: './task-page.component.scss'
})
export class TaskPageComponent {
  router = inject(ActivatedRoute)
  taskService = inject(TaskService)
  authService = inject(AuthService)
  notificationService = inject(NotificationService)
  commentService = inject(CommentService)
  task!: Task;
  comments$: Comment[] = [];
  commentForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.commentForm = this.fb.group({
      comment: ['', [Validators.required, Validators.minLength(5)]],
    });
  }

  ngOnInit(): void {
    const taskId = String(this.router.snapshot.paramMap.get('id'));
    this.taskService.getTaskById(taskId).subscribe(task => {
      this.task = task;
    });

    this.getcommentbycurrenttask(taskId);
  }

  getcommentbycurrenttask(taskId: string){
    this.commentService.getcommentbytask(taskId).subscribe(comments => {
      console.log(comments);
      this.comments$ = comments.sort((a, b) => new Date(b.createdOn).getTime() - new Date(a.createdOn).getTime())
    })
  }

  onCreateComment($event: Event) {
    const formData: CommentForm = { 
      taskId: this.task.id, 
      text: this.commentForm.value.comment 
    };
  
    // Подписка на результат запроса
    this.commentService.createcomment(formData).subscribe(
      complete => {
        this.commentForm.reset();
        this.getcommentbycurrenttask(this.task.id);
        
        console.log(this.authService.getCurrentUser())
        if(this.task.studentId == this.authService.getCurrentUser()?.nameid){
          return;
        }

        this.notificationService.createNotification({recipient: this.task.studentId, 
          notificationType: "2CBBCB67-FB42-4DCC-AE89-61F93A283D10", 
          text: `Добавлен комментарий для задания ${this.task.id}`
        }).subscribe(
          error => {
            console.error('Error creating notification', error);
          }
        );
      },
      error => {
        console.error('Error creating comment', error);
      }
    );
  
  }
}
