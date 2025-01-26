import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Notification } from '../interfaces/notification';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  http = inject(HttpClient);
  baseUrl = 'http://webapi:5000/';

  getNotifications(): Observable<Notification[]>{
    return this.http.get<Notification[]>(`${this.baseUrl}notification/getnotifications`);
  }

  createNotification(notification: { recipient: string, notificationType: string, text: string }): Observable<any> {
    console.log(notification)
    return this.http.post(`${this.baseUrl}notification/createnotification`, notification)
  }
}
