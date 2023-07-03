import { Component, OnInit } from '@angular/core';

import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {
  isAuth: boolean = false;
  userId: string = '';
  email: string = '';
  userName: string = '';
  firstName: string = '';
  lastName: string = '';
  subscriptionStatus: string = '';

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.isAuth = this.authService.checkAuth();
    if (this.isAuth) {
      this.userId = localStorage.getItem('UserId') || '';
      this.email = localStorage.getItem('Email') || '';
      this.userName = localStorage.getItem('UserName') || '';
      this.firstName = localStorage.getItem('FirstName') || '';
      this.lastName = localStorage.getItem('LastName') || '';
      this.subscriptionStatus = localStorage.getItem('SubscriptionStatus') || '';
    }
  }
}
