import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Role } from '../interfaces/role';
import { Observable } from 'rxjs';
import { ProfileForm } from '../interfaces/forms/profile-form.interfaces';
import { User } from '../interfaces/user';
import { ConfigService } from './config.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  http = inject(HttpClient);
  config = inject(ConfigService);

  getRoles(): Observable<Role[]>{
    return this.http.get<Role[]>(`${this.config.API_URL}role/getroles`);
  }

  getMe(): Observable<ProfileForm>{
    return this.http.get<ProfileForm>(`${this.config.API_URL}user/getme`)
  }

  getUsers(): Observable<ProfileForm[]> {
    return this.http.get<ProfileForm[]>(`${this.config.API_URL}user/getusers`)
  }

  getUserById(id :string): Observable<User> {
    return this.http.get<User>(`${this.config.API_URL}user/getuser/${id}`)
  }
}
