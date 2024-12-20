import { Component, inject} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Task } from '../../interfaces/task';
import { TaskService } from '../../services/task.service';
import { CommentComponent } from "../../common-ui/comment/comment.component";
import { NewCommentComponent } from "../../common-ui/new-comment/new-comment.component";

@Component({
  selector: 'app-task-page',
  standalone: true,
  imports: [CommentComponent, NewCommentComponent],
  templateUrl: './task-page.component.html',
  styleUrl: './task-page.component.scss'
})
export class TaskPageComponent {
  router = inject(ActivatedRoute)
  taskService = inject(TaskService)
  task$: Task | null = null;

  ngOnInit(): void {
    const taskId = String(this.router.snapshot.paramMap.get('id'));
    this.taskService.getTaskById(taskId).subscribe(task => {
      this.task$ = task;
    });
  }
}
