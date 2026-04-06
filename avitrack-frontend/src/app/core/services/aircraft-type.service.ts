import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AircraftTypeService {
  private apiUrl = '/api/aircraft-types';

  constructor(private http: HttpClient) {}

  add(icaoTypeCode: string, customLabel: string) {
    return this.http.post(this.apiUrl, { icaoTypeCode, customLabel });
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}