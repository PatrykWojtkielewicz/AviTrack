export interface FlightState {
  icao24: string;
  callsign: string;
  originCountry: string;
  longitude: number | null;
  latitude: number | null;
  altitude: number | null;
  velocity: number | null;
  heading: number | null;
  onGround: boolean;
}

export interface DashboardAirport {
  id: number;
  icaoCode: string;
  customLabel: string;
  name: string;
  city: string;
  country: string;
  latitude: number | null;
  longitude: number | null;
  nearbyFlights: FlightState[];
}

export interface DashboardFlight {
  id: number;
  callsign: string;
  customLabel: string;
  liveData: FlightState | null;
}

export interface DashboardAircraftType {
  id: number;
  icaoTypeCode: string;
  customLabel: string;
  liveFlights: FlightState[];
}

export interface DashboardResponse {
  airports: DashboardAirport[];
  flights: DashboardFlight[];
  aircraftTypes: DashboardAircraftType[];
}