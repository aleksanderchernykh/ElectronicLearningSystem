import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../auth/auth.service';
import { LoginForm } from '../../interfaces/forms/login-form';
import { Router, RouterOutlet } from '@angular/router';
import { catchError, throwError } from 'rxjs';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss'
})
export class LoginPageComponent {
  authService = inject(AuthService);
  router = inject(Router);
  userForm: FormGroup;
  error: string = "";

  constructor(private fb: FormBuilder) {
    this.userForm = this.fb.group({
      login: ['', [Validators.required, Validators.minLength(5)]],
      password: ['', [Validators.required, Validators.minLength(5)]],
    });
  }

  onSubmit(event: any) {
    const formData: LoginForm = this.userForm.value;

    this.authService.login(formData)
    .pipe(
      catchError(err=> {
        if (err.status === 401) {
          this.error = err.error.message;
        }
        
        return throwError(() => err);
      })
    )
    .subscribe({
      next: () => {
        this.router.navigate(['/']);
      },
    });
  }

  gotoRecoveryProfile() {
    const formData: LoginForm = this.userForm.value;
    
    if (formData.login) {
      this.router.navigate(['/recoveryprofile'], { queryParams: { login: formData.login } });
    } else {
      this.router.navigate(['/recoveryprofile']);
    }
  }
} 
