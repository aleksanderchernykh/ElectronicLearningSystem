import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CommentForm } from '../interfaces/forms/comment-form';
import { Comment } from '../interfaces/comment';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  http = inject(HttpClient);
  baseUrl = 'https://localhost:7291/';

  createcomment(payload: CommentForm): Observable<any>{
    return this.http.post(`${this.baseUrl}comment/createcomment`, payload);  
  }

  getcommentbytask(id: string): Observable<Comment[]>{
    return this.http.get<Comment[]>(`${this.baseUrl}comment/getcommentsbytask/${id}`);
  }
}
