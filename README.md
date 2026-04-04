# AviTrack

Aplikacja służąca śledzeniu ulubionych lotów, lotnisk oraz rodzajów samolotów

## Narzędzia

- Frontend: Angular 21.2.6, Tailwindcss
- Backend: .NET 10.0.201 Core Web API, Entity Framework Core, SQLite
- OpenSKy Network API

## Endpointy do API
**Autentykacja**

POST   `/api/auth/register`

POST   `/api/auth/login`

**Lotniska**

GET    `/api/airports`   

POST   `/api/airports`         

PUT    `/api/airports/{id}`

DELETE `/api/airports/{id}`

**Loty**

GET    `/api/flights`

POST   `/api/flights`

DELETE `/api/flights/{id}`

**Rodzaje samolotów**

GET    `/api/aircraft-types`

POST   `/api/aircraft-types`

DELETE `/api/aircraft-types/{id}`

**Strona główna**

GET    `/api/dashboard`
