import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';

import { provideHttpClient, withInterceptorsFromDi, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';
import { Login } from './features/auth/login/login';
import { FormsModule } from '@angular/forms';
import { Register } from './features/auth/register/register';
import { Dashboard } from './features/dashboard/dashboard';
import { Navbar } from './shared/components/navbar/navbar';

@NgModule({
  declarations: [App, Login, Register, Dashboard, Navbar],
  imports: [BrowserModule, AppRoutingModule, FormsModule],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(withInterceptorsFromDi()),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [App],
})
export class AppModule {}
