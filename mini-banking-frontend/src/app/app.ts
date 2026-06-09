import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoadingSpinner } from './shared/loading-spinner/loading-spinner';
import { Toast } from './shared/toast/toast';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, LoadingSpinner, Toast],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {}