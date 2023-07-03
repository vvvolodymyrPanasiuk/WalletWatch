import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})

export class HeaderComponent implements OnInit {

  isAuth: boolean = false;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.authService.authChange.subscribe((isAuth: boolean) => {
      this.isAuth = isAuth;
    });
  }

  logout(): void {
    localStorage.clear();
    this.authService.setAuthState(false);
    this.router.navigate(['/login']);
  }
}
