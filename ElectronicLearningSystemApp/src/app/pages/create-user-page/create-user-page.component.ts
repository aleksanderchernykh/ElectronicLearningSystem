import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-create-user-page',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule],
  templateUrl: './create-user-page.component.html',
  styleUrl: './create-user-page.component.scss'
})
export class CreateUserPageComponent {
  firstname: string = "";
  lastname: string = "";
  patronymic: string = "";
  type: string = "";
  userForm: FormGroup;

  constructor(private route: ActivatedRoute, private fb: FormBuilder) {  
    route.queryParams.subscribe(
      (queryParam: any) => {
          this.firstname = queryParam['firstname'];
          this.lastname = queryParam['lastname'];
          this.patronymic = queryParam['patronymic'];
          this.type = queryParam['type'];
      }
    );

    this.userForm = this.fb.group({
      firstname: [this.firstname, [Validators.required]],
      lastname: ['', [Validators.required]],
      patronymic: ['', [Validators.required]],
      typeuser: ['', [Validators.required]],
    });
  }

  onCreateUser($event: Event) {
    throw new Error('Method not implemented.');
  }
}
