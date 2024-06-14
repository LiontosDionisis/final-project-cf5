import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Router } from '@angular/router';


export interface LoginDto {
  username: string;
  password: string;
}

export interface JwtResponse {
  token: string;
  expires: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'http://localhost:5027/api/user'; 

  constructor(private http: HttpClient, private router: Router) { }


  login(dto: LoginDto): Observable<JwtResponse> {
    return this.http.post<JwtResponse>(`${this.apiUrl}/login`, dto).pipe(
      catchError(e => {
        return throwError(e);
      })
    )
  }

}
