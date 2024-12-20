import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Notification } from '../interfaces/notification';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  http = inject(HttpClient);
  baseUrl = 'https://localhost:7291/';

  getNotifications(): Observable<Notification[]>{
    return this.http.get<Notification[]>(`${this.baseUrl}notification/getnotifications`);
  }
}
