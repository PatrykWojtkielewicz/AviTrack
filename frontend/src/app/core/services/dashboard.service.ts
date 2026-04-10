import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DashboardResponse } from '../models/dashboard.model';

@Injectable({
    providedIn: 'root'
})
export class DashboardService {
    private apiUrl = '/api/dashboard';

    constructor(private http: HttpClient) {}

    getDashboard() {
        return this.http.get<DashboardResponse>(this.apiUrl);
    }
}