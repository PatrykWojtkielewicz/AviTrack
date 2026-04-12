import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
  standalone: false
})
export class Navbar implements OnInit {
  username = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.username = this.authService.getUsername() ?? '';
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  openUserSettings() {
    this.router.navigate(['/user']);
  }

  goToDashboard() {
    this.router.navigate(['/dashboard']);
  }
}