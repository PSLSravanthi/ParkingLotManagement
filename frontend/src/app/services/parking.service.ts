import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ParkingSlot, ParkVehicleRequest, SlotAvailability, Ticket } from '../models/parking.models';

@Injectable({
  providedIn: 'root'
})
export class ParkingService {
  private readonly baseUrl = `${environment.apiUrl}/parking`;

  constructor(private readonly http: HttpClient) {}

  getSlots(): Observable<ParkingSlot[]> {
    return this.http.get<ParkingSlot[]>(`${this.baseUrl}/slots`);
  }

  getAvailability(): Observable<SlotAvailability[]> {
    return this.http.get<SlotAvailability[]>(`${this.baseUrl}/availability`);
  }

  getActiveTickets(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(`${this.baseUrl}/tickets/active`);
  }

  parkVehicle(request: ParkVehicleRequest): Observable<Ticket> {
    return this.http.post<Ticket>(`${this.baseUrl}/park`, request);
  }

  exitVehicle(ticketId: number): Observable<Ticket> {
    return this.http.post<Ticket>(`${this.baseUrl}/exit/${ticketId}`, {});
  }
}
