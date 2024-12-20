import { Component, inject } from '@angular/core';
import { TaskService } from '../../services/task.service';
import { Task } from '../../interfaces/task';
import { Observable, of } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';


@Component({
  selector: 'app-board-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './board-page.component.html',
  styleUrl: './board-page.component.scss'
})
export class BoardPageComponent {
  taskService = inject(TaskService);
  router = inject(Router)
  tasks$: Task[] = [];

  ngOnInit(): void {
    this.taskService.getTasks()
    .subscribe(val =>{
      this.tasks$ = val
    })
  }

  goToTaskDetails(id: string) {
    this.router.navigate(['/task', id]);
  }
}
