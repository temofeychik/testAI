import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: `
    <main class="shell">
      <header class="shell__header">
        <h1>Quotes Manager</h1>
        <p class="subtitle">Импортированные цитаты PaperQuotes с CRUD</p>
      </header>
      <section class="shell__body">
        <router-outlet></router-outlet>
      </section>
    </main>
  `,
  styleUrls: ['./app.component.css'],
})
export class AppComponent {}
