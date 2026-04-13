import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs';
import { tap } from 'rxjs/operators';

type CachedFlight = {
  data: any;
  cachedAt: number;
};

@Injectable({
  providedIn: 'root'
})
export class FlightService {
  private apiUrl = '/api/flights';
  private cache = new Map<number, CachedFlight>();
  private readonly ttl = 60_000;

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
    const cachedFlight = this.cache.get(id);
    const isFresh = cachedFlight && Date.now() - cachedFlight.cachedAt < this.ttl;

    if (isFresh) {
      return of(cachedFlight.data);
    }

    return this.http.get(`${this.apiUrl}/${id}`).pipe(
      tap(data => {
        this.cache.set(id, {
          data,
          cachedAt: Date.now()
        });
      })
    );
  }

  invalidate(id?: number) {
    if (id !== undefined) {
      this.cache.delete(id);
      return;
    }

    this.cache.clear();
  }
}