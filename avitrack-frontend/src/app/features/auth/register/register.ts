import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.html',
  styleUrl: './register.css',
  standalone: false
})
export class Register {
  username = '';
  email = '';
  password = '';
  error = '';

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    this.authService.register(this.username, this.email, this.password).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: () => this.error = 'Rejestracja nie powiodła się'
    });
  }
}