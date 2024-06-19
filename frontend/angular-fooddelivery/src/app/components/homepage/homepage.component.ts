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
  //selectedCategory: Category | null = null;
  

  filteredFoods: any[] = [];

  constructor(private foodService: FoodService, private authService: AuthService){}

  ngOnInit(): void {
    this.loadFoodItems();
    //this.loadCategories();
    forkJoin([
      this.foodService.getFoodItems(),
      this.foodService.getCategories()
    ]).subscribe(([foodItemsResponse, categoriesResponse]) => {
      this.foodItems = foodItemsResponse.$values;
      this.categories = categoriesResponse.$values; 
      this.filteredFoods = this.foodItems;
    });
  }

  loadFoodItems() {
    this.foodService.getFoodItems().subscribe(data => {
      this.foodItems = data.$values; // Ensure the structure matches your API response
      this.categories = this.extractCategories(this.foodItems);
    });
  }

  // extractCategories(foodItems: any[]): Category[] {
    
  //   const categoriesMap = new Map<number, Category>();

  //   foodItems.forEach(food => {
  //     const categoryId = food.category.id;

  //     if (!categoriesMap.has(categoryId)) {
  //       categoriesMap.set(categoryId, {
  //         id: food.category.id,
  //         name: food.category.name,
  //         foods: []
  //       });
  //     }

  //     categoriesMap.get(categoryId)?.foods.push({
  //       id: food.id,
  //       name: food.name,
  //       price: food.price
  //     });
  //   });

  //   return Array.from(categoriesMap.values());
  // }

  extractCategories(foodItems: any[]): any[] {
    const categoriesMap = new Map<number, any>();
    foodItems.forEach(food => categoriesMap.set(food.category.id, food.category));
    return Array.from(categoriesMap.values());
  }

  onCategorySelect(category: any) {
    this.selectedCategory = category;
    this.selectedCategory.foods = this.extractFoodsByCategory(category.id);
  }

  extractFoodsByCategory(categoryId: number): any[] {
    return this.foodItems.filter(food => food.category.id === categoryId);
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

}
