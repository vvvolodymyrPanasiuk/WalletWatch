import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit {

  public loginForm!: FormGroup;
  public loading = false;
  public submitted = false;
  public returnUrl: string = "";

  constructor(private authService: AuthService, private formBuilder: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(8)]]
    })
  }

  onSubmit() {
    this.submitted = true;
    if (this.loginForm.invalid) {
      return;
    }
    this.loading = true;

    this.authService.login(this.loginForm.value)
      .subscribe(
        (response) => {
          alert('Login successful');
          console.log('Login successful', response);
          const userInfo = response;
          if (userInfo != null) {
            localStorage.setItem('Email', userInfo.email);
            localStorage.setItem('FirstName', userInfo.firstName);
            localStorage.setItem('LastName', userInfo.lastName);
            localStorage.setItem('SubscriptionStatus', userInfo.subscriptionStatus);
            localStorage.setItem('UserId', userInfo.userId);
            localStorage.setItem('UserName', userInfo.userName);
          }
          this.authService.setAuthState(true);
          this.router.navigate(['/home']);
        },
        (error) => {
          alert(error.error);
          console.log('Login error', error);
          this.loginForm.reset();
          this.loading = false;
        }
      );
  }
}
