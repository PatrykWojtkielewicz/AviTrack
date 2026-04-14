import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
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
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.currentUsername = this.authService.getUsername();
    this.form = this.fb.group({
      username: [this.currentUsername, [Validators.required, Validators.minLength(3)]]
    });

    if (!this.currentUsername) {
      this.authService.isLoggedIn().subscribe(() => {
        this.currentUsername = this.authService.getUsername();
        this.form.patchValue({ username: this.currentUsername });
      });
    }
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.loading = true;
    this.error = '';
    this.success = '';

    const newUsername = this.form.get('username')?.value;

    this.authService.updateUsername(newUsername).subscribe({
      next: () => {
        this.success = 'Nazwa użytkownika zaktualizowana pomyślnie';
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Nie udało się zaktualizować nazwy użytkownika';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  goBack() {
    this.router.navigate(['/dashboard']);
  }
}
