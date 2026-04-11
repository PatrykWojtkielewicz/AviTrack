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
  editMode = false;
  editingId: number | null = null;
  modalLoading = false;
  modalError = '';

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
      error: (err) => {
        if (err.status === 429) {
          this.error = 'Limit zapytań API ze strony OpenSky Network został osiągnięty. Spróbuj ponownie później';
        } else  {
          this.error = 'Nie udało się załadować danych';
        }

        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  openModal(type: ModalType) {
    this.activeModal = type;
    this.editMode = false;
    this.editingId = null;
    this.formIcao = '';
    this.formCallsign = '';
    this.formLabel = '';
    this.modalLoading = false;
    this.modalError = '';
  }

  openEditAirportModal(airport: any) {
    this.activeModal = 'airport';
    this.editMode = true;
    this.editingId = airport.id;
    this.formIcao = airport.icaoCode;
    this.formLabel = airport.customLabel;
    this.openDropdownId = null;
    this.modalLoading = false;
    this.modalError = '';
  }

  openEditFlightModal(flight: any) {
    this.activeModal = 'flight';
    this.editMode = true;
    this.editingId = flight.id;
    this.formCallsign = flight.callsign;
    this.formLabel = flight.customLabel;
    this.openDropdownId = null;
    this.modalLoading = false;
    this.modalError = '';
  }

  closeModal() {
    this.activeModal = null;
    this.modalLoading = false;
    this.modalError = '';
  }

  toggleDropdown(id: string) {
    this.openDropdownId = this.openDropdownId === id ? null : id;
  }

  submitModal() {
    if (this.modalLoading) return;
    this.modalLoading = true;
    this.modalError = '';

    if (this.activeModal === 'airport') {
      if (this.editMode && this.editingId !== null) {
        this.airportService.update(this.editingId, this.formLabel).subscribe({
          next: () => {
            this.modalLoading = false;
            this.closeModal();
            this.loadDashboard();
          },
          error: () => {
            this.modalLoading = false;
            this.modalError = 'Nie udało się zapisać lotniska';
            this.cdr.detectChanges();
          }
        });
      } else {
        this.airportService.add(this.formIcao, this.formLabel).subscribe({
          next: () => {
            this.modalLoading = false;
            this.closeModal();
            this.loadDashboard();
          },
          error: (err) => {
            this.modalLoading = false;
            this.modalError = err.status === 0 ? 'Nie udało połączyć się z serwerem. Spróbuj ponownie później' : 'Nie udało się zapisać lotniska';
            this.cdr.detectChanges();
          }
        });
      }
    } else if (this.activeModal === 'flight') {
      if (this.editMode && this.editingId !== null) {
        this.flightService.update(this.editingId, this.formLabel).subscribe({
          next: () => {
            this.modalLoading = false;
            this.closeModal();
            this.loadDashboard();
          },
          error: () => {
            this.modalLoading = false;
            this.modalError = 'Nie udało się zapisać lotu';
            this.cdr.detectChanges();
          }
        });
      } else {
        this.flightService.add(this.formCallsign, this.formLabel).subscribe({
          next: () => {
            this.modalLoading = false;
            this.closeModal();
            this.loadDashboard();
          },
          error: (err) => {
            this.modalLoading = false;
            this.modalError = err.status === 0 ? 'Nie udało połączyć się z serwerem. Spróbuj ponownie później' : 'Nie udało się zapisać lotu';
            this.cdr.detectChanges();
          }
        });
      }
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

  viewAirportDetails(airportId: number) {
    this.router.navigate(['/airports', airportId]);
  }
}