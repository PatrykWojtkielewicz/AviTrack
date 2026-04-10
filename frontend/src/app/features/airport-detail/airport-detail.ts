import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AirportService } from '../../core/services/airport.service';
import { ChangeDetectorRef } from '@angular/core';
import * as L from 'leaflet';

@Component({
  selector: 'app-airport-detail',
  standalone: false,
  templateUrl: './airport-detail.html',
  styleUrl: './airport-detail.css',
})
export class AirportDetail implements OnInit {
  airport: any = null;
  loading = true;
  error = '';
  private map: L.Map | null = null;
  private airportMarker!: L.Marker;
  private flightMarkers: L.Marker[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private airportService: AirportService,
    private cdr: ChangeDetectorRef
  ) {}

  @ViewChild('mapContainer') set mapContainer(el: ElementRef | undefined) {
    if(el && !this.map && this.airport) {
      this.initMap(el.nativeElement);
    }
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const airportId = params['id'];
      this.loadAirportDetails(airportId);
    });
  }

  loadAirportDetails(airportId: number) {
    this.loading = true;
    this.airportService.getById(airportId).subscribe({
      next: (airport) => {
        this.airport = airport;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Nie udało się załadować szczegółów lotniska';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  private initMap(container: HTMLElement) {
    const lat = this.airport.latitude;
    const lng = this.airport.longitude;

    this.map = L.map(container).setView([lat, lng], 10);
    
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors'
    }).addTo(this.map);

    const airportIcon = L.divIcon({
      html: `
        <div style="
          font-size: 32px;
          filter: drop-shadow(2px 2px 2px rgba(0,0,0,0.3));
        ">🛫</div>
      `,
      className: 'airport-marker',
      iconSize: [32, 32],
      iconAnchor: [16, 16]
    });

    this.airportMarker = L.marker([lat, lng], { icon: airportIcon })
      .addTo(this.map)
      .bindPopup(`
        <div style="text-align: center;">
          <strong>${this.airport.icaoCode}</strong><br>
          ${this.airport.name}
        </div>
      `);

    this.renderNearbyFlights();

    setTimeout(() => {
      this.map?.invalidateSize();
    }, 100);
  }

  private renderNearbyFlights() {
    this.flightMarkers.forEach(marker => marker.remove());
    this.flightMarkers = [];

    if (this.airport.nearbyFlights && this.airport.nearbyFlights.length > 0) {
      this.airport.nearbyFlights.forEach((flight: any) => {
        if (flight.latitude !== null && flight.longitude !== null) {
          const heading = flight.heading || 0;
          const planeIcon = L.divIcon({
            html: `
              <div style="
                transform: rotate(${heading-45}deg);
                font-size: 20px;
                filter: drop-shadow(2px 2px 2px rgba(0,0,0,0.3));
              ">✈️</div>
            `,
            className: 'plane-marker',
            iconSize: [20, 20],
            iconAnchor: [10, 10]
          });

          const marker = L.marker([flight.latitude, flight.longitude], { icon: planeIcon })
            .addTo(this.map!)
            .bindPopup(`
              <div style="text-align: center;">
                <strong>${flight.callsign}</strong><br>
                Wysokość: ${flight.altitude || '-'} m<br>
                Prędkość: ${flight.velocity ? (flight.velocity * 3.6).toFixed(0) : '-'} km/h
              </div>
            `);

          this.flightMarkers.push(marker);
        }
      });
    }
  }

  goBack() {
    this.router.navigate(['/dashboard']);
  }
}