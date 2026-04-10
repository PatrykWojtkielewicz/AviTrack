# AviTrack
A full-stack web application designed for tracking your favourite flights and airports
## Tech stack
| Layer | Technology |
|---|---|
| Frontend | Angular 21, Tailwind CSS |
| Backend | .NET 10, ASP.NET Core Web API |
| ORM | Entity Framework Core |
| Database | SQLite |
| External API | OpenSky Network |

## Running with Docker (recommended)

### Prerequisites
- [Docker](https://docs.docker.com/get-docker/)

### Steps

**1. Clone the repository**
```bash
git clone https://github.com/PatrykWojtkielewicz/AviTrack.git
cd AviTrack
```

**2. Configure secrets**
```bash
cp .env.example .env
```
Open `.env` and fill in your values:
```
JWT_KEY=your-jwt-secret-min-32-characters
OPENSKY_CLIENT_ID=your-opensky-client-id
OPENSKY_CLIENT_SECRET=your-opensky-client-secret
```

**3. Run**
```bash
docker compose up --build
```
The app will be available at `http://localhost`.

---

## Running locally (development)

### Prerequisites
- .NET SDK 10.0.201
- Node.js + npm
- [OpenSky Network](https://opensky-network.org/) account with API credentials

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