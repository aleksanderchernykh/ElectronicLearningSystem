import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { TokenResponse } from '../interfaces/token-response';
import { tap } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
 
export class AuthService {
  http: HttpClient = inject(HttpClient);
  baseUrl: string = "https://localhost:7291/";
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

  logout(){
    this.token = null;
    this.refreshToken = null;
    this.cookieService.deleteAll();
    this.route.navigateByUrl("/login");
  }

  login(payload: {login: string; password: string}){
    return this.http.post<TokenResponse>(`${this.baseUrl}auth/login`, payload)
      .pipe(
        tap(val=>{
          this.token = val.accessToken;
          this.refreshToken = val.refreshToken;

          this.cookieService.set('token', this.token);
          this.cookieService.set('refreshToken', this.refreshToken);
        })
      )  
  }
}