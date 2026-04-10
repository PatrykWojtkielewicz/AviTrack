import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = '/api/auth';
    private username: string | null = null;

    constructor(private http: HttpClient) {
        this.loadUsername();
    }

    register(username: string, email: string, password: string) {
        return this.http.post<{ username: string }>(`${this.apiUrl}/register`, { username, email, password }, { withCredentials: true })
            .pipe(tap(res => this.setUsername(res.username)));
    }

    login(email: string, password: string) {
        return this.http.post<{ username: string }>(`${this.apiUrl}/login`, { email, password }, { withCredentials: true })
            .pipe(tap(res => this.setUsername(res.username)));
    }

    logout() {
        return this.http.post(`${this.apiUrl}/logout`, {}, { withCredentials: true })
            .pipe(tap(() => {
                this.username = null;
                sessionStorage.removeItem('username');
            }));
    }

    getUsername() {
        return this.username;
    }

    private loadUsername() {
        this.username = sessionStorage.getItem('username');
    }

    isLoggedIn() {
        return !!this.username;
    }

    setUsername(username: string) {
        this.username = username;
        sessionStorage.setItem('username', username);
    }

    updateUsername(newUsername: string) {
        return this.http.put<{ username: string }>(`${this.apiUrl}/username`, { username: newUsername }, { withCredentials: true })
            .pipe(tap(res => this.setUsername(res.username)));
    }

}