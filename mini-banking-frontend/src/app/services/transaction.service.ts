import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environments';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Transactions`;

  transferByIban(data: any) {
    return this.http.post(`${this.apiUrl}/transfer-by-iban`, data);
  }

  getTransactions(accountId: number) {
    return this.http.get<any[]>(
      `${this.apiUrl}/account/${accountId}`
  );
}
}