import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-recovery-password-page',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './recovery-password-page.component.html',
  styleUrl: './recovery-password-page.component.scss'
})
export class RecoveryPasswordPageComponent {
  isConfirm: boolean = false;
  errorText: string = "";
  userId: string = "";
  token: string = "";
  recoveryForm: FormGroup;
  router = inject(Router);
  authService = inject(AuthService);

  constructor(private fb: FormBuilder, private route: ActivatedRoute) {
    this.recoveryForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(5)]],
    });

    this.route.queryParams.subscribe(params => {
      this.token = params['token'] || '';
      
      if(!this.token){
        this.router.navigate(['/login']);
      }

      this.userId = this.getUserByToken(this.token);
    });
  }

  getUserByToken(token: string): string {
    return "";
  }
}
