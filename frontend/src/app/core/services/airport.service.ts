import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs';
import { tap } from 'rxjs/operators';

type CachedAirport = {
  data: any;
  cachedAt: number;
};

@Injectable({
  providedIn: 'root'
})
export class AirportService {
  private apiUrl = '/api/airports';
  private cache = new Map<number, CachedAirport>();
  private readonly ttl = 60_000;

  constructor(private http: HttpClient) {}

  add(icaoCode: string, customLabel: string) {
    return this.http.post(this.apiUrl, { icaoCode, customLabel });
  }

  update(id: number, customLabel: string) {
    return this.http.put(`${this.apiUrl}/${id}`, { customLabel });
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getById(id: number) {
    const cachedAirport = this.cache.get(id);
    const isFresh = cachedAirport && Date.now() - cachedAirport.cachedAt < this.ttl;

    if (isFresh) {
      return of(cachedAirport.data);
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