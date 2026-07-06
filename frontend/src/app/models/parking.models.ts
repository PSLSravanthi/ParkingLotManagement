export type VehicleType = 'Motorcycle' | 'Car' | 'Bus';

export type SlotSize = 'Small' | 'Medium' | 'Large';

export interface ParkingSlot {
  id: number;
  size: SlotSize;
  isOccupied: boolean;
  vehicleTicketId: number | null;
}

export interface SlotAvailability {
  size: SlotSize;
  total: number;
  available: number;
}

export interface Ticket {
  id: number;
  vehicleNumber: string;
  vehicleType: VehicleType;
  slotId: number;
  entryTime: string;
  exitTime: string | null;
  fee: number | null;
  isActive: boolean;
}

export interface ParkVehicleRequest {
  vehicleNumber: string;
  vehicleType: VehicleType;
}
