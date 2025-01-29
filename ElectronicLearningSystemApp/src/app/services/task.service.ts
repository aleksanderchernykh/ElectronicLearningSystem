import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Task } from '../interfaces/task';
import { ConfigService } from './config.service';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  http = inject(HttpClient);
  config = inject(ConfigService);

  getTasks(): Observable<Task[]>{
    return this.http.get<Task[]>(`${this.config.API_URL}task/gettasks`);
  }

  getTaskById(id: string): Observable<Task>{
    return this.http.get<Task>(`${this.config.API_URL}task/gettask/${id}`);
  }
}
