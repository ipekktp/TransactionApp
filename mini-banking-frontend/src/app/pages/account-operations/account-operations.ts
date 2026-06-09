import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../services/account.service';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-account-operations',
  imports: [FormsModule, RouterLink],
  templateUrl: './account-operations.html',
  styleUrl: './account-operations.scss'
})
export class AccountOperations implements OnInit {
  private route = inject(ActivatedRoute);
  private accountService = inject(AccountService);
  private toastService = inject(ToastService);

  accountId!: number;
  amount: number | null = null;
  selectedOperation: 'deposit' | 'withdraw' = 'deposit';
  resultMessage = '';

  ngOnInit(): void {
    this.accountId = Number(this.route.snapshot.paramMap.get('accountId'));
  }

  submitOperation() {
    this.resultMessage = '';

    const data = {
      accountId: this.accountId,
      amount: this.amount
    };

    const request =
      this.selectedOperation === 'deposit'
        ? this.accountService.deposit(data)
        : this.accountService.withdraw(data);

    request.subscribe({
      next: (response: any) => {
        this.toastService.success(response.message);
        this.resultMessage = `Yeni bakiye: ₺${response.balance}`;
        this.amount = null;
      },
      error: (err) => {
        this.toastService.error(err.error || 'İşlem sırasında hata oluştu.');
      }
    });
  }
}