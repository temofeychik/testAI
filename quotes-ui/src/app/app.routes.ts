import { Routes } from '@angular/router';
import { QuotesPageComponent } from './quotes-page.component';

export const appRoutes: Routes = [
  { path: '', component: QuotesPageComponent },
  { path: '**', redirectTo: '' },
];
