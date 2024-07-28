import { Component, inject } from '@angular/core';
import { UserServiseService } from '../../services/user-servise.service';
import { Role } from '../../interfaces/role';

@Component({
  selector: 'app-profile-page',
  standalone: true,
  imports: [],
  templateUrl: './profile-page.component.html',
  styleUrl: './profile-page.component.scss'
})
export class ProfilePageComponent {
  userServices = inject(UserServiseService);
  roles: Role[] = []
  constructor(){
    this.userServices.getRoles()
      .subscribe(val => {
        this.roles = val
      })
  }
}
