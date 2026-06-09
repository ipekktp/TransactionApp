import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TransactionService } from '../../services/transaction.service';

@Component({
  selector: 'app-transactions',
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './transactions.html',
  styleUrl: './transactions.scss'
})
export class Transactions implements OnInit {
  private route = inject(ActivatedRoute);
  private transactionService = inject(TransactionService);
  private cdr = inject(ChangeDetectorRef);

  transactions: any[] = [];
  accountId!: number;
  isLoaded = false;

  searchText = '';
  directionFilter = 'All';

  ngOnInit(): void {
    this.accountId = Number(this.route.snapshot.paramMap.get('accountId'));

    setTimeout(() => {
      this.transactionService.getTransactions(this.accountId).subscribe({
        next: (response) => {
          this.transactions = response;
          this.isLoaded = true;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.log('TRANSACTION ERROR:', err);
          this.transactions = [];
          this.isLoaded = true;
          this.cdr.detectChanges();
        }
      });
    }, 0);
  }

  get filteredTransactions() {
    return this.transactions.filter(t => {
      const direction = (t.direction || '').toLowerCase();
      const search = this.searchText.toLowerCase();

      const matchesSearch =
        t.transactionType?.toLowerCase().includes(search) ||
        t.amount?.toString().includes(search) ||
        t.transactionDate?.toLowerCase().includes(search);

      const matchesDirection =
        this.directionFilter === 'All' ||
        direction === this.directionFilter.toLowerCase();

      return matchesSearch && matchesDirection;
    });
  }
}