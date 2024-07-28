import { Component, inject, signal, ViewChild, WritableSignal } from '@angular/core';
import { UserServiseService } from '../../services/user-servise.service';
import { ProfileForm } from '../../interfaces/forms/profile-form.interfaces';
import { map, Observable, of } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Role } from '../../interfaces/role';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfigService } from '../../services/config.service';

@Component({
  selector: 'app-user-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './user-page.component.html',
  styleUrl: './user-page.component.scss'
})
export class UserPageComponent {
  userService = inject(UserServiseService);
  const = inject(ConfigService);
  users$: Observable<ProfileForm[]> = of([]);
  roles$: Observable<Role[] | null> = of(null);
  filerUsers: ProfileForm[] = [];
  userForm: FormGroup;
  filterUsers: ProfileForm[] = [];
  urlForCreateUser: WritableSignal<string | null> = signal(null);
  baseUrl: string = 'http://localhost:4200/';

  constructor(private fb: FormBuilder) {
    this.userForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      patronymic: ['', [Validators.required]],
      typeUser: ['', [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.users$ = this.userService.getUsers();

    this.users$.subscribe(val =>{
      this.filerUsers = val
    })

    this.roles$ = this.userService.getRoles();
  }

  onCreateUser($event: Event) : void {
    var form = this.userForm.value; 
    this.urlForCreateUser.set(`${this.baseUrl}createuser?firstname=${form.lastName}&lastname=${form.lastName}&patronymic=${form.patronymic}&type=${form.typeUser}`);
  }

  closeModelCreatingUser() : void {
    this.userForm.reset();
    this.urlForCreateUser.set(null);
  }

  handleChange($event: Event) {
    if(($event.target as HTMLInputElement).checked){
      this.filerUsers = this.filerUsers.filter(position => position.isLocked == false)
    }else{
      this.users$.subscribe(val =>{
        this.filerUsers = val
      })
    }

    console.log(this.filerUsers);
  }
}
