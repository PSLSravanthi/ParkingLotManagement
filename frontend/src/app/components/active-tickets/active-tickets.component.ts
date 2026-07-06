import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Ticket } from '../../models/parking.models';

@Component({
  selector: 'app-active-tickets',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './active-tickets.component.html',
  styleUrl: './active-tickets.component.css'
})
export class ActiveTicketsComponent {
  @Input({ required: true }) tickets: Ticket[] = [];
  @Output() exit = new EventEmitter<number>();
}
