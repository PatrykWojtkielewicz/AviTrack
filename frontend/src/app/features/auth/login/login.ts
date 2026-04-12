import { Component, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrl: './login.css',
  standalone: false
})
export class Login {
  email = '';
  password = '';
  error = '';
  loading = false;
  showPassword = false;

  constructor(private authService: AuthService, private router: Router, private cdr: ChangeDetectorRef) {}

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  onSubmit() {
    if (this.loading) return;
    this.error = '';
    this.loading = true;

    this.authService.login(this.email, this.password).subscribe({
      next: (res) => {
        this.authService.setUsername(res.username);
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        if (err.status === 0) {
          this.error = 'Nie udało połączyć się z serwerem. Spróbuj ponownie później';
        } else if (err.status === 409) {
          this.error = 'Nieprawidłowy adres e-mail lub hasło';
        } else {
          this.error = 'Coś poszło nie tak. Spróbuj ponownie później';
        }

        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }
}