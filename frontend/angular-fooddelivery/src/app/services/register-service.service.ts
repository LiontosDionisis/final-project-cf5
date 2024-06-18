import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Router } from '@angular/router';


export interface InsertDto {
  username: string;
  password: string;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class RegisterServiceService {

  private apiUrl = 'http://localhost:5027/api/user'; 

  constructor(private http: HttpClient, private router: Router) { }

  register(dto: InsertDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, dto).pipe(
      catchError(e => {
        return throwError(e);
      })
    )
  }
}
