import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { CommentForm } from '../interfaces/forms/comment-form';
import { Comment } from '../interfaces/comment';


@Injectable({
  providedIn: 'root'
})
export class CommentService {
  http = inject(HttpClient);
  baseUrl = 'http://webapi:5000/';

  createcomment(payload: CommentForm): Observable<any>{
    console.log(payload);
    return this.http.post(`${this.baseUrl}comment/createcomment`, payload).pipe(
      catchError(this.handleError) // Обработка ошибок
    );
  }

  getcommentbytask(id: string): Observable<Comment[]>{
    return this.http.get<Comment[]>(`${this.baseUrl}comment/getcommentsbytask/${id}`);
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // Ошибки на стороне клиента (например, проблемы с сетью)
      console.log('An error occurred:', error.error.message);
    } else {
      // Ошибки на стороне сервера
      console.log(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`
      );
    }

    // Возвращаем Observable с описанием ошибки
    return throwError(() => new Error('Something went wrong; please try again later.'))
  }
}
