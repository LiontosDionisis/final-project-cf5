import { Component, OnInit } from '@angular/core';
import { FoodService } from '../../services/food.service';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { RouterLink, RouterModule } from '@angular/router';
import { AuthService } from '../../services/login-service.service';
import { forkJoin } from 'rxjs';
import { Category } from '../../services/food.service';
import { OrderService } from '../../services/order.service';
import { OrderInsertDTO, OrderReadOnlyDTO } from '../../models/dtos';
import { FormsModule } from '@angular/forms';

interface FoodItem {
  id: number;
  name: string;
  price: number;
  category: Category;
}

export interface CartItem {
  id: number;
  name: string;
  price: number;
  quantity: number;
  category : Category;
}

@Component({
  selector: 'app-homepage',
  standalone: true,
  imports: [CommonModule, HttpClientModule, RouterLink, RouterModule, FormsModule],
  templateUrl: './homepage.component.html',
  styleUrl: './homepage.component.css',
  providers: [
    FoodService,
    AuthService,
    OrderService
  ]
})

export class HomepageComponent implements OnInit {

  isNavbarCollapsed = true;
  foodItems: FoodItem[] = [];
  categories: any[] = [];
  selectedCategory: any = null;
  cartItems: CartItem[] = [];
  address: string = '';
  filteredFoods: any[] = [];
  orderMsg: string = '';
  ErrOrderMsg: string = '';

  constructor(private foodService: FoodService, private authService: AuthService, private orderService: OrderService){}


  ngOnInit(): void {
    forkJoin([
      this.foodService.getFoodItems(),
      this.foodService.getCategories()
    ]).subscribe(([foodItemsResponse, categoriesResponse]) => {
      const allFoodItems = foodItemsResponse.$values.map((foodItem: { category: { foods: { $values: any[]; }; id: any; name: any; }; }) => {
        if (foodItem.category && foodItem.category.foods && foodItem.category.foods.$values) {
          return foodItem.category.foods.$values.map(food => ({
            ...food,
            category: { id: foodItem.category.id, name: foodItem.category.name }
          }));
        }
        return [foodItem];
      }).flat();
  
      this.foodItems = allFoodItems;
      this.categories = categoriesResponse.$values;
      this.filteredFoods = this.foodItems;
    });
  }

  loadFoodItems() {
    this.foodService.getFoodItems().subscribe(data => {
      const allFoodItems = data.$values.flatMap((foodItem: { category: { foods: { $values: any[]; }; id: any; name: any; }; }) => {
        if (foodItem && foodItem.category && foodItem.category.foods && foodItem.category.foods.$values) {
          return foodItem.category.foods.$values.map(food => ({
            ...food,
            category: { id: foodItem.category.id, name: foodItem.category.name }
          }));
        }
        return [];
      });

      this.foodItems = allFoodItems;
      this.categories = this.extractCategories(this.foodItems);
    });
  }
  
  extractCategories(foodItems: any[]): any[] {
    const categoriesMap = new Map<number, any>();
    foodItems.forEach(food => {
      if (food.category && food.category.id != null) {
        categoriesMap.set(food.category.id, food.category);
      }
    });
  
    const categories = Array.from(categoriesMap.values());
    return categories;
  }
  
  onCategorySelect(category: any) {
    if (category && category.id != null) {
      this.selectedCategory = category;
      this.filteredFoods = this.extractFoodsByCategory(category.id);
    } else {
      console.warn('Invalid category selected:', category);
    }
  }
  
  extractFoodsByCategory(categoryId: number): any[] {
    const filtered = this.foodItems.filter(food => food.category && food.category.id === categoryId);
    return filtered;
  }


  toggleNavbar() {
    this.isNavbarCollapsed = !this.isNavbarCollapsed;
  }

  logout(): void {
    if (window.confirm("Are you sure you want to logout?")) {
      this.authService.logout();
    }
  }

  isLoggedIn() {
    return this.authService.isLoggedIn();
  }

  closeNavbar() {
    this.isNavbarCollapsed = true;
  }

  addToCart(food: any) {
    const existingItem = this.cartItems.find(item => item.id === food.id);
  
    if (existingItem) {
      existingItem.quantity++;
    } else {
      this.cartItems.push({
        id: food.id,
        name: food.name,
        price: food.price,
        quantity: 1,
        category: food.category 
      });
    }
  }
  
  removeFromCart(cartItem: CartItem): void {
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

  placeOrder(address: string): void {
    const userId = this.authService.getUserIdFromToken();
    if (!userId) {
      console.error('User ID not found in JWT token');
      return;
    }
  
    const dto: OrderInsertDTO = {
      userId: userId,
      address: address,
      price: this.getTotalPrice(),
      items: this.cartItems.map(item => ({
        name: item.name,
        price: item.price,
        categoryId: item.category.id 
      }))
    };
  
    this.orderService.placeOrder(dto).subscribe(
      (order) => {
        console.log('Order placed successfully:', order);
        this.orderMsg = "Order was sent! Your doorbell will ring in 25 minutes.";
        this.cartItems = [];
      },
      (error) => {
        console.error('Error placing order:', error);
        this.ErrOrderMsg = "Something went wrong :( Please try again.";
      }
    );
  }
}
