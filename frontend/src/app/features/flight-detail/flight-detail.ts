import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
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
  private map: L.Map | null = null;
  private planeMarker!: L.Marker;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private flightService: FlightService,
    private cdr: ChangeDetectorRef
  ) {}

  @ViewChild('mapContainer') set mapContainer(el: ElementRef | undefined) {
    if(el && !this.map) {
      this.initMap(el.nativeElement);
    }
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const flightId = params['id'];
      this.loadFlightDetails(flightId);
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

  
private initMap(container: HTMLElement) {
  const lat = this.flight.liveData.latitude;
  const lng = this.flight.liveData.longitude;
  const heading = this.flight.liveData.heading || 0;

  this.map = L.map(container).setView([lat, lng], 10);
  
  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '© OpenStreetMap contributors'
  }).addTo(this.map);

  this.map.scrollWheelZoom.disable();

  const planeIcon = L.divIcon({
    html: `
      <div style="
        transform: rotate(${heading-45}deg);
        font-size: 28px;
        filter: drop-shadow(2px 2px 2px rgba(0,0,0,0.3));
      ">✈️</div>
    `,
    className: 'plane-marker',
    iconSize: [28, 28],
    iconAnchor: [14, 14]
  });

  this.planeMarker = L.marker([lat, lng], { icon: planeIcon })
    .addTo(this.map)
    .bindPopup(`
      <div style="text-align: center;">
        <strong>${this.flight.callsign}</strong><br>
        Wysokość: ${this.flight.liveData.altitude || '-'} m<br>
        Prędkość: ${this.flight.liveData.velocity ? (this.flight.liveData.velocity * 3.6).toFixed(0) : '-'} km/h
      </div>
    `);

  setTimeout(() => {
    this.map?.invalidateSize();
  }, 100);
}

updatePlanePosition(lat: number, lng: number, heading: number) {
  if (this.planeMarker) {
    this.planeMarker.setLatLng([lat, lng]);

    const icon = this.planeMarker.getElement();
    if (icon) {
      const innerDiv = icon.querySelector('div');
      if (innerDiv) {
        innerDiv.style.transform = `rotate(${heading}deg)`;
      }
    }
  }
}

  goBack() {
    this.router.navigate(['/dashboard']);
  }
}