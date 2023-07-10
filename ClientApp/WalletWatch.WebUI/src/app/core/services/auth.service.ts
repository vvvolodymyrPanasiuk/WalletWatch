import { environment } from './../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = environment.apiUrl + '/Authentication';
  authChange = new Subject<boolean>();

  constructor(private http: HttpClient) { }

  register(registerData: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, registerData);
  }

  login(loginData: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, loginData);
  }

  checkAuth(): boolean {
    let userID = localStorage.getItem('UserId');
    console.log(userID);
    return userID !== null;
  }

  setAuthState(isAuth: boolean): void {
    this.authChange.next(isAuth);
  }
}
