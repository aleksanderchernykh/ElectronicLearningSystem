import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Task } from '../interfaces/task';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  http = inject(HttpClient);
  baseUrl = 'https://localhost:7291/';

  getTasks(): Observable<Task[]>{
    return this.http.get<Task[]>(`${this.baseUrl}task/gettasks`);
  }

  getTaskById(id: string): Observable<Task>{
    return this.http.get<Task>(`${this.baseUrl}task/gettask/${id}`);
  }
}
