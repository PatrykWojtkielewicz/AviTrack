import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FlightService } from '../../core/services/flight.service';
import { ChangeDetectorRef } from '@angular/core';
import * as L from 'leaflet';

@Component({
  selector: 'app-flight-detail',
  standalone: false,
  templateUrl: './flight-detail.html',
  styleUrl: './flight-detail.css',
})
export class FlightDetail implements OnInit {
  flight: any = null;
  loading = true;
  error = '';
  private map: L.Map | undefined;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private flightService: FlightService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      const flightId = params['id'];
      this.loadFlightDetails(flightId);
      this.loadMap();
    });
  }

  loadFlightDetails(flightId: number) {
    this.loading = true;
    this.flightService.getById(flightId).subscribe({
      next: (flight) => {
        this.flight = flight;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Nie udało się załadować szczegółów lotu';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  loadMap() {
    this.map = L.map('map').setView([52.237, 21.017], 6);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors'
    }).addTo(this.map);
  }

  goBack() {
    this.router.navigate(['/dashboard']);
  }
}