import { Component } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { AuthService } from '../../services/login-service.service';
import { FoodService } from '../../services/food.service';

interface FoodItem {
  id: number;
  name: string;
  price: number;
}

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [HttpClientModule, CommonModule, RouterLink, RouterModule],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css',
  providers: [
    AuthService,
    FoodService
  ]
})
export class AdminComponent {
  foodItems: FoodItem[] = [];

  isNavbarCollapsed = true;


  constructor(private authService: AuthService, private router: Router, private foodService: FoodService) { }

  ngOnInit(): void {
    const userRole = this.authService.getUserRole();
    if (userRole !== "admin") {
      this.router.navigate(['/login']);
    }
    if (userRole !== "admin" && this.authService.isLoggedIn()) {
      this.router.navigate(['/home']);
    }
    this.loadFoodItems();
  }

  toggleNavbar() {
    this.isNavbarCollapsed = !this.isNavbarCollapsed;
  }

  closeNavbar() {
    this.isNavbarCollapsed = true;
  }

  logout(): void {
    if (window.confirm("Are you sure you want to logout?")) {
      this.authService.logout();
    }
  }

  isLoggedIn() {
    return this.authService.isLoggedIn();
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
