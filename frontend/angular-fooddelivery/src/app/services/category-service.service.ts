import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';

export interface CategoryInsertDto {
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class CategoryServiceService {

  private apiUrl = "http://localhost:5027/api/category"

  constructor(private http: HttpClient) { }

  getCategories(): Observable<any> {
    return this.http.get<any>('http://localhost:5027/api/category');
  }
  
  addCategory(dto: CategoryInsertDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, dto).pipe(
      catchError(error => {
        return throwError(error);
      })
    )
  }
}
