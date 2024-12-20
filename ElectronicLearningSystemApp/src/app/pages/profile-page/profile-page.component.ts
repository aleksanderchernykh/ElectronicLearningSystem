import { Component, inject } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Role } from '../../interfaces/role';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-profile-page',
  standalone: true,
  imports: [],
  templateUrl: './profile-page.component.html',
  styleUrl: './profile-page.component.scss'
})
export class ProfilePageComponent {
  userServices = inject(UserService);
  roles: Role[] = []
  constructor(){
    this.userServices.getRoles()
      .subscribe(val => {
        this.roles = val
      })
  }
}
