import { Routes } from '@angular/router';
import { AccountOperations } from './pages/account-operations/account-operations';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { Dashboard } from './pages/dashboard/dashboard';
import { Transfer } from './pages/transfer/transfer';
import { Transactions } from './pages/transactions/transactions';
import { authGuard } from './guards/auth-guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  { path: 'login', component: Login },
  { path: 'register', component: Register },

  { path: 'dashboard', component: Dashboard, canActivate: [authGuard] },
  { path: 'transfer/:accountId', component: Transfer, canActivate: [authGuard] },
  { path: 'transactions/:accountId', component: Transactions, canActivate: [authGuard] },
  { path: 'account-operations/:accountId', component: AccountOperations, canActivate: [authGuard] },

  { path: '**', redirectTo: 'login' }

];