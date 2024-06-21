import { Component } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { AuthService } from '../../services/login-service.service';
import { Category, FoodInsertDto, FoodService } from '../../services/food.service';
import { FormsModule } from '@angular/forms';
import { CategoryInsertDto, CategoryServiceService } from '../../services/category-service.service';

interface FoodItem {
  id: number;
  name: string;
  price: number;
}



@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [HttpClientModule, CommonModule, RouterLink, RouterModule, FormsModule],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css',
  providers: [
    AuthService,
    FoodService,
    CategoryServiceService
  ]
})
export class AdminComponent {


  errorMessage = ''


  insertDto: FoodInsertDto = {
    name: "",
    price: 0,
    category: ""
  }

  catInsertDto: CategoryInsertDto = {
    name: ""
  }
  
  categories: Category[] = [];
  foodItems: FoodItem[] = [];
  isNavbarCollapsed = true;
  selectedFood: any = null;
  isUpdateFormVisible = false;


  constructor(private catService: CategoryServiceService, private authService: AuthService, private router: Router, private foodService: FoodService) { }

  ngOnInit(): void {
    const userRole = this.authService.getUserRole();
    if (userRole !== "admin") {
      this.router.navigate(['/login']);
    }
    if (userRole !== "admin" && this.authService.isLoggedIn()) {
      this.router.navigate(['/home']);
    }
    this.loadFoodItems();
    this.loadCategories();
  }

  onSubmitCat() {
    this.catService.addCategory(this.catInsertDto).subscribe(
      (response) => {
        window.location.reload();
      },
      (error) => {
      }
    )
  }

  onSubmitFood() {
    this.foodService.addFoodItem(this.insertDto).subscribe(
      (response) => {
        window.location.reload();
      },
      (error) => {
        if (error.status == 400){
          this.errorMessage = "Error inserting food. Make sure category is inserted."
        }
      }
    )
  }

  onDelete(food: any): void {
    const confirmed = window.confirm(`Are you sure you want to delete ${food.name}?`);
    if (confirmed) {
      this.foodService.deleteFood(food.id).subscribe(
        (response) => {
          window.location.reload();
        },
        (error) => {
          console.error("Error deleting food", error);
        }
      );
    }
  }

  onUpdate(food: any): void {
    this.selectedFood = { ...food };
    this.isUpdateFormVisible = true;
  }

  onSubmitUpdate(): void {
    if (this.selectedFood) {
      this.foodService.updateFood(this.selectedFood.id, this.selectedFood).subscribe(
        response => {
          this.isUpdateFormVisible = false;
          this.loadFoodItems();
        },
        error => {
          console.error('Error updating food item', error);
        }
      );
    }
  }

  onCancelUpdate(): void {
    this.isUpdateFormVisible = false;
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

  loadCategories(): void {
    this.foodService.getCategories().subscribe(
      (response: any) => {
        if (response && response.$values) {
          this.categories = response.$values;
        } 
      },
      (error) => {
        console.error('Error loading categories:', error);
      }
    );
  }

  loadFoodItems() {
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
