import { Component, OnInit } from '@angular/core';
import { ThemeService } from './core/services/theme.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  standalone: false
})
export class App implements OnInit {
  title = 'avitrack-frontend';

  constructor(private themeService: ThemeService) {}

  ngOnInit() { }
}