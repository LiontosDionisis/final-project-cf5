import { Component } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { AuthService } from '../../services/login-service.service';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [HttpClientModule, CommonModule, RouterLink, RouterModule],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css',
  providers: [
    AuthService
  ]
})
export class AdminComponent {

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    const userRole = this.authService.getUserRole();
    if (userRole !== "admin") {
      this.router.navigate(['/login']);
    }
    if (userRole !== "admin" && this.authService.isLoggedIn()) {
      this.router.navigate(['/home']);
    }
  }
}
