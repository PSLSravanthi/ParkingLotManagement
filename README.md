# hello-world

I am Sravanthi pursuing B tech 3rd year at Aditya College of Engineering And Technology.
I am much intrested in codding.

## Parking Lot Management System

A full-stack parking lot management system with a .NET (ASP.NET Core) backend and an
Angular frontend.

### Features

- Park a vehicle (motorcycle, car, or bus) and receive a ticket
- Vehicles are assigned the smallest slot that fits them, falling back to a larger
  slot when the preferred size is full
- Exit a vehicle to compute the parking fee (hourly rate, billed by slot size)
- Live dashboard showing slot availability and active tickets

### Project structure

```
backend/    ASP.NET Core Web API (C#) - parking domain logic and REST endpoints
frontend/   Angular app - dashboard UI for parking/exiting vehicles
```

### Running the backend

Requires the [.NET 8 SDK](https://dotnet.microsoft.com/download).

```bash
cd backend
dotnet test              # run unit tests
dotnet run --project ParkingLot.Api --urls http://localhost:5080
```

The API is served at `http://localhost:5080/api/parking` with Swagger UI available at
`http://localhost:5080/swagger` in development.

### Running the frontend

Requires Node.js and npm.

```bash
cd frontend
npm install
npm start                # serves on http://localhost:4200
```

The frontend expects the backend to be running at `http://localhost:5080` (configured
in `frontend/src/environments/environment.ts`).

### API endpoints

| Method | Endpoint                        | Description                     |
|--------|----------------------------------|----------------------------------|
| GET    | `/api/parking/slots`             | List all slots and their state  |
| GET    | `/api/parking/availability`      | Slot counts by size              |
| GET    | `/api/parking/tickets/active`    | List currently parked vehicles  |
| POST   | `/api/parking/park`              | Park a vehicle, returns a ticket |
| POST   | `/api/parking/exit/{ticketId}`   | Exit a vehicle, returns the fee  |
