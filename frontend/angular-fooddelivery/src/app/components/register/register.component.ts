import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Route, Router, RouterLink, RouterModule } from '@angular/router';
import { InsertDto, RegisterServiceService } from '../../services/register-service.service';
import { AuthService } from '../../services/login-service.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [HttpClientModule, FormsModule, CommonModule, RouterLink, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
  providers: [
    RegisterServiceService,
    AuthService
  ]
})
export class RegisterComponent {

  insertDto: InsertDto = {
    username: "",
    password: "",
    email: ""
  };

  constructor(private registerService: RegisterServiceService, private router: Router, private authService: AuthService) {}

  ngOnInit() : void {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/home']);
    }
  }
  
  onSubmit() {
    this.registerService.register(this.insertDto).subscribe(
      (response) => {
        console.log("Registration successfull");
        this.router.navigate(['/login']);
      },
      (error) => {
        console.error("Registration Failed", error);
      }
    )
  }
}
