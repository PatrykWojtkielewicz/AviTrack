import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DashboardResponse } from '../models/dashboard.model';
import { of } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class DashboardService {
    private apiUrl = '/api/dashboard';
    private cache: DashboardResponse | null = null;
    private cachedAt = 0;
    private readonly ttl = 60_000;

    constructor(private http: HttpClient) {}

    getDashboard(force = false) {
        const isFresh = this.cache && Date.now() - this.cachedAt < this.ttl;

        if (!force && isFresh) {
            return of(this.cache!);
        }

        return this.http.get<DashboardResponse>(this.apiUrl).pipe(
            tap(data => {
                this.cache = data;
                this.cachedAt = Date.now();
            })
        );
    }

    invalidate() {
        this.cache = null;
        this.cachedAt = 0;
    }
}