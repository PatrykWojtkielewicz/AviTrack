# AviTrack

A full-stack web application designed for tracking your favourite flights nad airports

## Tech stack

| Layer | Technology |
|---|---|
| Frontend | Angular 21, Tailwind CSS |
| Backend | .NET 10, ASP.NET Core Web API |
| ORM | Entity Framework Core |
| Database | SQLite |
| External API | OpenSky Network |

## Prerequisites

- .NET SDK 10.0.201
- Node.js + npm
- [OpenSky Network](https://opensky-network.org/) account with API credentials

## Getting started

### 1. Clone the repository

```bash
git clone https://github.com/PatrykWojtkielewicz/AviTrack.git
cd AviTrack
```

### 2. Configure secrets:

 
Copy the example config file:
 
```bash
cp backend/appsettings.json backend/appsettings.Development.json
```
 
Then open `backend/appsettings.Development.json` and fill in your values:
 
```json
{
  "Jwt": {
    "Key": "YOUR_JWT_SECRET_KEY_MIN_32_CHARACTERS"
  },
  "OpenSky": {
    "ClientId": "YOUR_OPENSKY_CLIENT_ID",
    "ClientSecret": "YOUR_OPENSKY_CLIENT_SECRET"
  }
}
```
 
- **Jwt.Key** — any random string, at least 32 characters long
- **OpenSky.ClientId / ClientSecret** — obtained from your OpenSky Network account

### 3. Run the backend

```bash
cd backend
dotnet restore
dotnet run
```

### 4. Run the frontend

```bash
cd frontend
npm install
npm start
```