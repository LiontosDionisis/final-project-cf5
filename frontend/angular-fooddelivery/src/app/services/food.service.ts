import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, tap, throwError } from 'rxjs';

export interface FoodInsertDto {
  name: string;
  price: number;
  category: string;
}


export interface Category {
  id: number;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class FoodService {

  private apiUrl = 'http://localhost:5027/api/food';

  constructor(private http:HttpClient) { }


  getFoodItems(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}`);
  }

  getCategories(): Observable<any> {
    return this.http.get<any>('http://localhost:5027/api/category');
  }

  addFoodItem(dto: FoodInsertDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, dto).pipe(
      catchError(error => {
        return throwError(error);
      })
    );
  }

  deleteFood(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }

}
  
