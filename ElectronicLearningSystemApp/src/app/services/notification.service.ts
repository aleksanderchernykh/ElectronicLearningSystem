import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Notification } from '../interfaces/notification';
import { ConfigService } from './config.service';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  http = inject(HttpClient);
  config = inject(ConfigService);

  getNotifications(): Observable<Notification[]>{
    return this.http.get<Notification[]>(`${this.config.API_URL}notification/getnotifications`);
  }

  createNotification(notification: { recipient: string, notificationType: string, text: string }): Observable<any> {
    console.log(notification)
    return this.http.post(`${this.config.API_URL}notification/createnotification`, notification)
  }
}
