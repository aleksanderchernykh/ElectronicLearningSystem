import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { TokenResponse } from '../interfaces/token-response';
import { catchError, tap, throwError } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { currentuser } from '../interfaces/currentuser';
import { jwtDecode } from 'jwt-decode';
import { ConfigService } from '../services/config.service';

@Injectable({
  providedIn: 'root'
})
 
export class AuthService {
  http: HttpClient = inject(HttpClient);
  config = inject(ConfigService);
  cookieService = inject(CookieService)
  token: string | null = null;
  refreshToken: string | null = null;
  route = inject(Router)

  getIsAuth(){
    if(!this.token){
      this.token = this.cookieService.get('token')
    }
    
    return !!this.token;
  }

  getToken(): string | null {
    return this.cookieService.get('token');
  }

  getCurrentUser(): currentuser | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      return jwtDecode<currentuser>(token);
    } catch (error) {
      console.error('Ошибка при декодировании токена:', error);
      return null;
    }
  }

  logout(){
    this.token = null;
    this.refreshToken = null;
    this.cookieService.deleteAll();
    this.route.navigateByUrl("/login");
  }

  login(payload: { login: string; password: string }) {
    console.log('Login request started', { payload }); // Логирование начальных данных
  
    return this.http.post<TokenResponse>(`${this.config.API_URL}auth/login`, payload)
      .pipe(
        tap(val => {
          console.log('Login response received', val); // Логирование ответа от сервера
  
          this.token = val.accessToken;
          this.refreshToken = val.refreshToken;
  
          this.cookieService.set('token', this.token);
          this.cookieService.set('refreshToken', this.refreshToken);
  
          console.log('Token and refresh token saved in cookies'); // Логирование сохранения токенов
        }),
        catchError(error => {
          console.error('Error during login', error); // Логирование ошибки
          return error; // Пробрасываем ошибку дальше
        })
      );
  }
}