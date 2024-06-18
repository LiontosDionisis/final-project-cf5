import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FoodService {

  private apiUrl = 'http://localhost:5027/api/food';

  constructor(private http:HttpClient) { }


  getFoodItems(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}`);
  }

}
  
