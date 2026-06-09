import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  isLoading = signal(false);

  show() {
    setTimeout(() => {
      this.isLoading.set(true);
    });
  }

  hide() {
    setTimeout(() => {
      this.isLoading.set(false);
    });
  }
}