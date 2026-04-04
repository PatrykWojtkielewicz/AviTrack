import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AirportService {
  private apiUrl = 'http://localhost:5251/api/airports';

  constructor(private http: HttpClient) {}

  add(icaoCode: string, customLabel: string) {
    return this.http.post(this.apiUrl, { icaoCode, customLabel });
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}