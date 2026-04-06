import { Component, ChangeDetectorRef } from '@angular/core';
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

  rules = [
    { label: 'Minimum 8 znaków', met: false, check: (p: string) => p.length >= 8 },
    { label: 'Jedna wielka litera', met: false, check: (p: string) => /[A-Z]/.test(p) },
    { label: 'Jedna cyfra',        met: false, check: (p: string) => /[0-9]/.test(p) },
  ];

  get passwordValid() {
    return this.rules.every(r => r.met);
  }

  onPasswordChange() {
    this.rules.forEach(r => r.met = r.check(this.password));
  }

  constructor(private authService: AuthService, private router: Router, private cdr: ChangeDetectorRef) {}

  onSubmit() {
    if (!this.passwordValid) {
      this.error = 'Hasło nie spełnia wymagań';
      return;
    }

    this.error = '';
    this.authService.register(this.username, this.email, this.password).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: (err) => {
        if (err.status === 0) {
          this.error = 'Nie udało połączyć się z serwerem. Spróbuj ponownie później';
        } else if (err.status === 409) {
          this.error = 'Ten adres e-mail jest już zajęty';
        } else {
          this.error = 'Coś poszło nie tak. Spróbuj ponownie później';
        }
        this.cdr.detectChanges();
      }
    });
  }
}