import { Component, inject, OnInit } from '@angular/core';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TransactionService } from '../../services/transaction.service';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-transfer',
  imports: [FormsModule, RouterLink],
  templateUrl: './transfer.html',
  styleUrl: './transfer.scss'
})
export class Transfer implements OnInit {
  private transactionService = inject(TransactionService);
  private route = inject(ActivatedRoute);
  private toastService = inject(ToastService);

  fromAccountId!: number;
  toIban = '';
  amount: number | null = null;

  successMessage = '';
  errorMessage = '';

  ngOnInit(): void {
    this.fromAccountId = Number(
      this.route.snapshot.paramMap.get('accountId')
    );
  }

  sendTransfer() {
  this.successMessage = '';
  this.errorMessage = '';

  const data = {
    fromAccountId: this.fromAccountId,
    toIban: this.toIban,
    amount: this.amount
  };

  this.transactionService.transferByIban(data).subscribe({
    next: (response: any) => {
      this.toastService.success('IBAN ile transfer başarılı.');

      this.successMessage =
        `Yeni bakiye: ₺${response.fromAccountBalance}`;

      this.toIban = '';
      this.amount = null;
    },
    error: (err) => {
      this.toastService.error(err.error || 'Transfer sırasında hata oluştu.');
    }
  });
}
}