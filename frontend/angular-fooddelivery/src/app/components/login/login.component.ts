import { Component } from '@angular/core';
import { LoginDto, AuthService } from '../../services/login-service.service';
import { Router, RouterLink, RouterModule } from '@angular/router';
import {FormsModule} from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [HttpClientModule, FormsModule, CommonModule, RouterLink, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  providers: [
    AuthService
  ]
})

export class LoginComponent {

  errorMessage = ""

  loginDto: LoginDto = {
    username: "",
    password: ""
  };
  

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() : void {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/home']);
    }
  }

  onSubmit(): void {
    this.authService.login(this.loginDto).subscribe(
      (response) => {
        console.log('Login successful', response);
        const userRole = this.authService.getUserRole();
        if (userRole === 'admin') {
          this.router.navigate(['/admin']); 
        } else {
          this.router.navigate(['/']);
        }
      },
      (error) => {
        console.error('Login failed', error);
        if (error.status == 404){
          this.errorMessage = "Invalid credentials."
        }
        if (error.status == 401){
          this.errorMessage = "Invalid credentials."
        }
        if (error.status == 500){
          this.errorMessage = "Internal server error."
        }
      }
    );
  }
}
