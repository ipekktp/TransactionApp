import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-register',
  imports: [FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.scss'
})
export class Register {
  private authService = inject(AuthService);
  private router = inject(Router);
  private toastService = inject(ToastService);

  username = '';
  email = '';
  password = '';

  successMessage = '';
  errorMessage = '';

  register() {
    this.successMessage = '';
    this.errorMessage = '';

    const data = {
      username: this.username,
      email: this.email,
      password: this.password
    };

    this.authService.register(data).subscribe({
      next: () => {
        this.toastService.success('Kayıt başarılı.');
        this.router.navigate(['/login']);
      },
      error: (err) => {
      this.toastService.error(err.error || 'Kayıt sırasında hata oluştu.');
      }
    });
  }
}