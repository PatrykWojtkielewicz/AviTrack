import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { Dashboard } from './features/dashboard/dashboard';
import { FlightDetail } from './features/flight-detail/flight-detail';
import { AirportDetail } from './features/airport-detail/airport-detail';
import { User } from './features/user/user';

const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: 'dashboard', component: Dashboard },
  { path: 'flights/:id', component: FlightDetail },
  { path: 'airports/:id', component: AirportDetail },
  { path: 'user', component: User },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
