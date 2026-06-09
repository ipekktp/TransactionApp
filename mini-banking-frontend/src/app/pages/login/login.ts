import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  private authService = inject(AuthService);
  private router = inject(Router);
  private toastService = inject(ToastService);

  username = '';
  password = '';
  errorMessage = '';

  login() {
  const data = {
    username: this.username,
    password: this.password
  };

  this.authService.login(data).subscribe({
    next: () => {
      this.toastService.success('Giriş başarılı.');
      this.router.navigate(['/dashboard']);
    },
    error: () => {
      this.toastService.error('Kullanıcı adı veya şifre hatalı.');
    }
  });
}
}