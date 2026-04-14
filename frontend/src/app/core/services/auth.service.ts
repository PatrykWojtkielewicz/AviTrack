import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { tap, map, catchError } from 'rxjs/operators';

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

    isLoggedIn(): Observable<boolean> {
        return this.http.get<{ username: string }>(`${this.apiUrl}/me`, { withCredentials: true }).pipe(
            tap(res => this.setUsername(res.username)),
            map(() => true),
            catchError(() => {
                this.username = null;
                sessionStorage.removeItem('username');
                return of(false);
            })
        );
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