import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { AuthService } from '../../auth/auth.service';
import { NotificationService } from '../../services/notification.service';
import { Notification } from '../../interfaces/notification';
import * as signalR from "@microsoft/signalr";

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterModule, RouterOutlet],
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit, OnDestroy {
  route = inject(Router);
  authService = inject(AuthService);
  notificationsService = inject(NotificationService);

  notifications: Notification[] = [];
  notificats: Notification[] = [];
  connection!: signalR.HubConnection;

  constructor() {}

  ngOnInit(): void {
    this.notificationsService.getNotifications().subscribe({
      next: (val) => {
        this.notifications = val;
        console.log(this.notifications);
      },
      error: (err) => console.error("Error fetching notifications:", err),
    });

    this.initializeSignalRConnection();
  }

  private initializeSignalRConnection(): void {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7291/notificationhub", {
        accessTokenFactory: () => this.authService.getToken() || ''
      })
      .configureLogging(signalR.LogLevel.Information)
      .withAutomaticReconnect()
      .build();

    this.connection
      .start()
      .then(() => console.log("SignalR is connected"))
      .catch((err) => console.error("Error connecting to SignalR:", err));

    this.connection.on('Message', (message) => {
      console.log(message);
    });

    this.connection.on('CreatedNotificationFromUser', (message) => {
      console.log("Notification received: ", message);
    });

    this.connection.onclose((error) => {
      console.error("SignalR connection closed:", error);
    });
  }

  createNotification(): void {
    if (this.connection?.state === signalR.HubConnectionState.Connected) {
      this.connection.invoke('CreatedNotification', 'sdfffsdf')
        .catch(err => console.error("Error sending notification: ", err));
    } else {
      console.warn("SignalR connection is not established.");
    }
  }

  clearAllNotification() {
    throw new Error('Method not implemented.');
  }

  logout(): void {
    this.authService.logout();
  }

  ngOnDestroy(): void {
    if (this.connection) {
      this.connection.stop().then(() => console.log("SignalR connection stopped"));
    }
  }
}
