import { Component, OnInit } from '@angular/core';
import { FoodService } from '../../services/food.service';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { RouterLink, RouterModule } from '@angular/router';
import { AuthService } from '../../services/login-service.service';

interface FoodItem {
  id: number;
  name: string;
  price: number;
}

interface Category {
  id: number;
  name: string;
}

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

  foodItems: FoodItem[] = [];

  constructor(private foodService: FoodService, private authService: AuthService){}

  ngOnInit(): void {
    this.loadFoodItems()
  }

  isNavbarCollapsed = true;

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

  private loadFoodItems() {
    this.foodService.getFoodItems().subscribe(
      (response: any) => {
        if (response && response['$values']) {
          // Extracting food items from the $values array
          this.foodItems = response['$values'].map((item: any) => {
            return {
              id: item.id,
              name: item.name,
              price: item.price
            };
          });
        }
      },
      error => {
        console.error('Error fetching food items:', error);
      }
    );
  }

}
