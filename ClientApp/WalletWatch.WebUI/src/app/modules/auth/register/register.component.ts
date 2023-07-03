import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {

  registerForm!: FormGroup;
  loading = false;
  submitted = false;
  constructor(private authService: AuthService, private formBuilder: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      passwordConfirm: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  onSubmit() {
    this.submitted = true;
    if (this.registerForm.invalid) {
      return;
    }

    this.loading = true;

    this.authService.register(this.registerForm.value)
      .subscribe(
        () => {
          alert('Registration successful');
          console.log('Registration successful');
          this.router.navigate(['/login']);
        },
        (error) => {
          alert(error.error);
          console.log('Registration error', error);
          this.registerForm.reset();
          this.loading = false;
        }
      );
  }
}
