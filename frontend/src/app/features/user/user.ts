import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user',
  standalone: false,
  templateUrl: './user.html',
  styleUrl: './user.css',
})
export class User implements OnInit {
  form!: FormGroup;
  loading = false;
  error = '';
  success = '';
  currentUsername: string | null = null;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.currentUsername = this.authService.getUsername();
    this.form = this.fb.group({
      username: [this.currentUsername, [Validators.required, Validators.minLength(3)]]
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.loading = true;
    this.error = '';
    this.success = '';

    const newUsername = this.form.get('username')?.value;

    // TODO: Add API call to update username on backend
    // For now, just update locally
    this.authService.setUsername(newUsername);
    this.success = 'Nazwa użytkownika zaktualizowana pomyślnie';
    this.loading = false;
  }

  goBack() {
    this.router.navigate(['/dashboard']);
  }
}
