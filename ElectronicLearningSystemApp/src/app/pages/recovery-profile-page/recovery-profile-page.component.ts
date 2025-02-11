import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../auth/auth.service';
import { catchError, throwError } from 'rxjs';

@Component({
  selector: 'app-recovery-profile-page',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './recovery-profile-page.component.html',
  styleUrl: './recovery-profile-page.component.scss'
})
export class RecoveryProfilePageComponent {
  isConfirm: boolean = false;
  errorText: string = "";
  login: string = "";
  recoveryForm: FormGroup;
  authService = inject(AuthService);

  constructor(private fb: FormBuilder, private route: ActivatedRoute) {
    this.recoveryForm = this.fb.group({
      login: ['', [Validators.required, Validators.minLength(5)]],
    });

    this.route.queryParams.subscribe(params => {
      this.login = params['login'] || '';
  
      if (this.login) {
        this.recoveryForm.patchValue({ login: this.login });
      }
    });
  }

  gotoRecoveryProfile() {
    const login = this.recoveryForm.value;
    this.errorText = "";
  
    this.authService.recoveryPassword(login)
      .pipe(
        catchError(err=> {
          if (err.status === 404) {
            this.errorText = err.error.errorMessage;
          }
        
          return throwError(() => err);
        })
      )
      .subscribe({
        next: () => {
          this.errorText = "";   
          this.isConfirm = true;
        },
    });
  }
}
