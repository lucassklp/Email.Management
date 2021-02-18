import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import jwtDecode from 'jwt-decode';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private http: HttpClient) { }

  public authenticate(email: string, password: string): Observable<string> {
    return this.http.post<{token: string}>('', {email, password}).pipe(map(x => x.token))
  }

  public set token(value: string){
    sessionStorage.setItem('token', value)
  }

  public get token(): string {
    return sessionStorage.getItem('token') || '';
  }

  public get isAuthenticated(): boolean {
    return this.token != null && this.token != undefined && this.token != '';
  }

  public get user(): any {
    let user = jwtDecode(this.token);
    console.log(user);
    return user;
  }

  public logout(){
    localStorage.removeItem('token')
  }
}
