import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CartItem } from '../components/homepage/homepage.component';
import { OrderInsertDTO, OrderReadOnlyDTO } from '../models/dtos';

@Injectable({
  providedIn: 'root'
})

export class OrderService {

  private cartItems: CartItem[] = [];
  private apiUrl = 'http://localhost:5027/api/order';

  constructor(private http: HttpClient) { }

  

  // addToCart(food: any) {
  //   const existingItem = this.cartItems.find(item => item.id === food.id);

  //   if (existingItem) {
  //     existingItem.quantity++;
  //   } else {
  //     this.cartItems.push({
  //       id: food.id,
  //       name: food.name,
  //       price: food.price,
  //       quantity: 1
  //     });
  //   }
  // }

  removeFromCart(cartItem: CartItem) {
    const index = this.cartItems.findIndex(item => item.id === cartItem.id);

    if (index !== -1) {
      if (this.cartItems[index].quantity > 1) {
        this.cartItems[index].quantity--;
      } else {
        this.cartItems.splice(index, 1);
      }
    }
  }

  getTotalPrice(): number {
    return this.cartItems.reduce((total, item) => total + (item.price * item.quantity), 0);
  }

  getCartItems(): CartItem[] {
    return this.cartItems;
  }

  clearCart() {
    this.cartItems = [];
  }


  placeOrder(dto: OrderInsertDTO): Observable<OrderReadOnlyDTO> {
    return this.http.post<OrderReadOnlyDTO>(this.apiUrl, dto);
  }
}
