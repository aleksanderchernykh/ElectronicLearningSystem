import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Role } from '../interfaces/role';
import { Observable } from 'rxjs';
import { ProfileForm } from '../interfaces/forms/profile-form.interfaces';
import { User } from '../interfaces/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  http = inject(HttpClient);
  baseUrl = 'http://webapi:5000/';

  getRoles(): Observable<Role[]>{
    return this.http.get<Role[]>(`${this.baseUrl}role/getroles`);
  }

  getMe(): Observable<ProfileForm>{
    return this.http.get<ProfileForm>(`${this.baseUrl}user/getme`)
  }

  getUsers(): Observable<ProfileForm[]> {
    return this.http.get<ProfileForm[]>(`${this.baseUrl}user/getusers`)
  }

  getUserById(id :string): Observable<User> {
    return this.http.get<User>(`${this.baseUrl}user/getuser/${id}`)
  }
}
