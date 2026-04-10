import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class FlightService {
  private apiUrl = '/api/flights';

  constructor(private http: HttpClient) {}

  add(callsign: string, customLabel: string) {
    return this.http.post(this.apiUrl, { callsign, customLabel });
  }

  update(id: number, customLabel: string) {
    return this.http.put(`${this.apiUrl}/${id}`, { customLabel });
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getById(id: number) {
    return this.http.get(`${this.apiUrl}/${id}`);
  }
}