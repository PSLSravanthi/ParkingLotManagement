import { Component, OnInit } from '@angular/core';
import { ParkingService } from '../../services/parking.service';
import { ParkVehicleRequest, SlotAvailability, Ticket } from '../../models/parking.models';
import { ParkVehicleFormComponent } from '../park-vehicle-form/park-vehicle-form.component';
import { ActiveTicketsComponent } from '../active-tickets/active-tickets.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [ParkVehicleFormComponent, ActiveTicketsComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  availability: SlotAvailability[] = [];
  activeTickets: Ticket[] = [];
  statusMessage: string | null = null;
  errorMessage: string | null = null;

  constructor(private readonly parkingService: ParkingService) {}

  ngOnInit(): void {
    this.refresh();
  }

  refresh(): void {
    this.parkingService.getAvailability().subscribe((availability) => (this.availability = availability));
    this.parkingService.getActiveTickets().subscribe((tickets) => (this.activeTickets = tickets));
  }

  onPark(request: ParkVehicleRequest): void {
    this.errorMessage = null;
    this.parkingService.parkVehicle(request).subscribe({
      next: (ticket) => {
        this.statusMessage = `Parked ${ticket.vehicleNumber} in slot #${ticket.slotId} (ticket #${ticket.id}).`;
        this.refresh();
      },
      error: (err) => {
        this.errorMessage = err?.error?.message ?? 'Could not park the vehicle. The lot may be full.';
      }
    });
  }

  onExit(ticketId: number): void {
    this.errorMessage = null;
    this.parkingService.exitVehicle(ticketId).subscribe({
      next: (ticket) => {
        this.statusMessage = `${ticket.vehicleNumber} exited. Fee: $${ticket.fee?.toFixed(2)}.`;
        this.refresh();
      },
      error: (err) => {
        this.errorMessage = err?.error?.message ?? 'Could not process the exit.';
      }
    });
  }
}
