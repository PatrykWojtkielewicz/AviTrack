import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { DashboardService } from '../../core/services/dashboard.service';
import { AirportService } from '../../core/services/airport.service';
import { FlightService } from '../../core/services/flight.service';
import { DashboardResponse } from '../../core/models/dashboard.model';
import { Router } from '@angular/router';

type ModalType = 'airport' | 'flight' | 'aircraftType' | null;

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
  standalone: false
})
export class Dashboard implements OnInit {
  data: DashboardResponse | null = null;
  loading = true;
  error = '';

  activeModal: ModalType = null;
  openDropdownId: string | null = null;

  formIcao = '';
  formCallsign = '';
  formLabel = '';

  constructor(
    private dashboardService: DashboardService,
    private airportService: AirportService,
    private flightService: FlightService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadDashboard();
  }

  loadDashboard() {
    this.loading = true;
    this.dashboardService.getDashboard().subscribe({
      next: (res) => {
        this.data = res;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Nie udało się załadować danych';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  openModal(type: ModalType) {
    this.activeModal = type;
    this.formIcao = '';
    this.formCallsign = '';
    this.formLabel = '';
  }

  closeModal() {
    this.activeModal = null;
  }

  toggleDropdown(id: string) {
    this.openDropdownId = this.openDropdownId === id ? null : id;
  }

  submitModal() {
    if (this.activeModal === 'airport') {
      this.airportService.add(this.formIcao, this.formLabel).subscribe(() => {
        this.closeModal();
        this.loadDashboard();
      });
    } else if (this.activeModal === 'flight') {
      this.flightService.add(this.formCallsign, this.formLabel).subscribe(() => {
        this.closeModal();
        this.loadDashboard();
      });
    }
  }

  deleteAirport(id: number) {
    this.airportService.delete(id).subscribe(() => this.loadDashboard());
    this.openDropdownId = null;
  }

  deleteFlight(id: number) {
    this.flightService.delete(id).subscribe(() => this.loadDashboard());
    this.openDropdownId = null;
  }

  viewFlightDetails(flightId: number) {
    this.router.navigate(['/flights', flightId]);
  }
}