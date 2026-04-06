import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = '/api/auth';

    constructor(private http: HttpClient) {}

    register(username: string, email: string, password: string) {
        return this.http.post<{ token: string, username: string}>(`${this.apiUrl}/register`, { username, email, password})
            .pipe(tap(res => this.saveToken(res)));
    }

    login(email: string, password: string) {
        return this.http.post<{ token: string, username: string }>(`${this.apiUrl}/login`, { email, password })
            .pipe(tap(res => this.saveToken(res)));
    }

    private saveToken(res: { token: string, username: string}) {
        localStorage.setItem('token', res.token);
        localStorage.setItem('username', res.username);
    }

    logout() {
        localStorage.removeItem('token');
        localStorage.removeItem('username');
    }

    getToken() {
        return localStorage.getItem('token');
    }
    
    getUsername() {
        return localStorage.getItem('username');
    }
    
    isLoggedIn() {
        return !!this.getToken();
    }

}