import { Component, inject, Input } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../interfaces/user';

@Component({
  selector: 'app-mini-profile',
  standalone: true,
  imports: [],
  templateUrl: './mini-profile.component.html',
  styleUrl: './mini-profile.component.scss'
})
export class MiniProfileComponent {
  @Input() userId!: string;
  userService = inject(UserService);
  user!: User;
  
  ngOnInit(): void {
    this.userService.getUserById(this.userId).subscribe(user => {
      this.user = user
    })
  }
}
