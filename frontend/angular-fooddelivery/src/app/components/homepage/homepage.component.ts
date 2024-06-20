import { Component, OnInit } from '@angular/core';
import { FoodService } from '../../services/food.service';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { RouterLink, RouterModule } from '@angular/router';
import { AuthService } from '../../services/login-service.service';
import { forkJoin } from 'rxjs';
import { Category } from '../../services/food.service';

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
}


// interface Category {
//   id: number;
//   name: string;
// }

@Component({
  selector: 'app-homepage',
  standalone: true,
  imports: [CommonModule, HttpClientModule, RouterLink, RouterModule],
  templateUrl: './homepage.component.html',
  styleUrl: './homepage.component.css',
  providers: [
    FoodService,
    AuthService
  ]
})

export class HomepageComponent implements OnInit {

  isNavbarCollapsed = true;
  foodItems: FoodItem[] = [];
  categories: any[] = [];
  selectedCategory: any = null;
  cartItems: CartItem[] = [];

  //selectedCategory: Category | null = null;
  

  filteredFoods: any[] = [];

  constructor(private foodService: FoodService, private authService: AuthService){}


  ngOnInit(): void {
    forkJoin([
      this.foodService.getFoodItems(),
      this.foodService.getCategories()
    ]).subscribe(([foodItemsResponse, categoriesResponse]) => {
      console.log('Food Items Response:', foodItemsResponse.$values); // Log the food items
      console.log('Categories Response:', categoriesResponse.$values); // Log the categories
  
      // Flatten the food items
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
      this.filteredFoods = this.foodItems; // Initialize with all food items
  
      this.foodItems.forEach(food => {
        console.log('Food:', food.name, 'Category ID:', food.category.id);
      });
    });
  }

  loadFoodItems() {
    this.foodService.getFoodItems().subscribe(data => {
      // Flatten the food items
      const allFoodItems = data.$values.map((foodItem: { category: { foods: { $values: any[]; }; id: any; name: any; }; }) => {
        if (foodItem.category && foodItem.category.foods && foodItem.category.foods.$values) {
          return foodItem.category.foods.$values.map(food => ({
            ...food,
            category: { id: foodItem.category.id, name: foodItem.category.name }
          }));
        }
        return [foodItem];
      }).flat();

      this.foodItems = allFoodItems;
      this.categories = this.extractCategories(this.foodItems);
    });
  }

  extractCategories(foodItems: any[]): any[] {
    const categoriesMap = new Map<number, any>();
    foodItems.forEach(food => categoriesMap.set(food.category.id, food.category));
    return Array.from(categoriesMap.values());
  }

  onCategorySelect(category: any) {
    this.selectedCategory = category;
    this.filteredFoods = this.extractFoodsByCategory(category.id); // Update filteredFoods
    console.log('Selected Category:', this.selectedCategory); // Log the selected category
    console.log('Filtered Foods:', this.filteredFoods); // Log the filtered foods
  }

  extractFoodsByCategory(categoryId: number): any[] {
    const filtered = this.foodItems.filter(food => food.category.id === categoryId);
    console.log('Filtered Foods for Category ID', categoryId, ':', filtered); // Log the filtered foods
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
    // Check if the item is already in the cart
    const existingItem = this.cartItems.find(item => item.id === food.id);
  
    if (existingItem) {
      // If item exists, increase the quantity
      existingItem.quantity++;
    } else {
      // If item doesn't exist, add it to the cart
      this.cartItems.push({
        id: food.id,
        name: food.name,
        price: food.price,
        quantity: 1
      });
    }
  
    // Optionally, you can notify the user that the item was added to the cart
    console.log(`${food.name} added to cart!`);
  }


  removeFromCart(cartItem: CartItem) {
    const index = this.cartItems.findIndex(item => item.id === cartItem.id);
  
    if (index !== -1) {
      if (this.cartItems[index].quantity > 1) {
        // If quantity > 1, decrease the quantity
        this.cartItems[index].quantity--;
      } else {
        // If quantity === 1, remove the item from the cart
        this.cartItems.splice(index, 1);
      }
    }
  
    // Optionally, you can notify the user that the item was removed from the cart
    console.log(`${cartItem.name} removed from cart!`);
  }

  getTotalPrice(): number {
    return this.cartItems.reduce((total, item) => total + (item.price * item.quantity), 0);
  }

}
