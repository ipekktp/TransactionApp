import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environments';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);

  private apiUrl = `${environment.apiUrl}/Account`;

  getMyAccounts() {
    return this.http.get<any[]>(`${this.apiUrl}/my-accounts`);
  }

  createMyAccount() {
    return this.http.post(`${this.apiUrl}/create-my-account`, {});
  }

  deposit(data: any) {
    return this.http.post(`${this.apiUrl}/deposit`, data);
  }

  withdraw(data: any) {
    return this.http.post(`${this.apiUrl}/withdraw`, data);
  }
}