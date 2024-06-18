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

  loginDto: LoginDto = {
    username: "",
    password: ""
  };
  

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    this.authService.login(this.loginDto).subscribe(
      (response) => {
        console.log("Login successfull");
        this.router.navigate(['/home']);
      },
      (error) => {
        console.error("Login failed", error);
      }
    )
  }
}
