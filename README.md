<div align="center">

# ZentrixBarber

**A modern barbershop booking platform built with ASP.NET Core MVC**

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET_Core-MVC-512BD4?style=flat-square)
![EF Core](https://img.shields.io/badge/EF_Core-10.0-6C3483?style=flat-square)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-7952B3?style=flat-square&logo=bootstrap&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)

</div>

---

## About

ZentrixBarber is a web application for barbershops that moves appointment booking online.
Instead of calling the shop and spending several minutes arranging a time slot, clients can browse the barbers,
check the services and prices, and book an appointment on their own - while the barbers keep working without interruptions.

> This project is developed as the course assignment for **ASP.NET Fundamentals** at SoftUni and is actively in progress.

---

## Features

### Implemented

- **Barbers list** - all active barbers shown as cards with years of experience and number of offered services
- **Barber profile** - detailed page with bio, experience and a table of the services the barber offers, including duration and price
- **Services and prices** - full price list grouped by category (Haircut, Beard Grooming, Combo), rendered through a reusable partial view
- **Online booking** - clients book an appointment by choosing a barber, a service, a date and a time; booking directly from a barber's profile pre-selects the barber
- **Full appointment management (CRUD)** - list of upcoming appointments, detailed view, editing and deletion with a confirmation page
- **Appointment status workflow** - appointments move from Pending to Confirmed to Completed, or can be Cancelled; only valid transitions are allowed
- **Server-side and client-side validation** - data annotations with custom messages, plus business rules checked against the database:
  - the appointment must be in the future
  - the whole service must fit within working hours (09:00 - 20:00)
  - the selected barber must offer the selected service
  - the barber cannot be double-booked - overlapping appointments are rejected
- **Soft delete for barbers** - inactive barbers are hidden from the public pages and cannot be booked, without losing their appointment history
-   - **Service layer** - business logic and database queries live in services behind interfaces, registered through dependency injection; controllers stay thin
- **Seeded database** - the application starts with ready-to-use barbers, categories, services and barber-service relations

### Planned

- Administrator login - only the shop administrator can manage the schedule
- Custom dark theme, animations and responsive design improvements

---

## Technologies

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 10) |
| ORM | Entity Framework Core 10 |
| Database | Microsoft SQL Server |
| Front-end | Razor Views, Bootstrap 5 |
| Architecture | MVC, View Models, Fluent API configurations, Dependency Injection |

---

## Database Schema

| Entity | Description |
|---|---|
| `Barber` | A barber working in the shop - name, bio, experience, active status |
| `TreatmentCategory` | Groups services into categories such as Haircut or Beard Grooming |
| `Treatment` | A service offered by the shop - name, description, price, duration |
| `BarberTreatment` | Many-to-many mapping between barbers and the services they offer |
| `Appointment` | A client booking - client details, date and time, status, barber and service |

Deleting a barber or a service is restricted when appointments exist, so booking history is never lost.

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Microsoft SQL Server (Express, Developer or LocalDB)
- Visual Studio 2022 / 2026 or JetBrains Rider

### 1. Clone the repository

```bash
git clone https://github.com/B-Evtimov/ZentrixBarber.git
cd ZentrixBarber
```

### 2. Configure the database connection

The connection string is located in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ZentrixBarber;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

The default configuration uses a local SQL Server instance with Windows Authentication.
If your instance has a different name, change the `Server` value:

| SQL Server setup | Value |
|---|---|
| Default local instance | `Server=.` |
| SQL Server Express | `Server=.\\SQLEXPRESS` |
| LocalDB | `Server=(localdb)\\MSSQLLocalDB` |

No other environment variables or credentials are required.

### 3. Apply the migrations

Using the **Package Manager Console** in Visual Studio:

```powershell
Update-Database
```

Or using the .NET CLI:

```bash
dotnet ef database update
```

This creates the `ZentrixBarber` database and fills it with the initial seed data.

### 4. Run the application

Press **F5** in Visual Studio, or:

```bash
dotnet run
```

---

## Project Structure

```
ZentrixBarber/
├── Common/            Validation constants shared across entities and view models
├── Controllers/       MVC controllers (Home, Barbers, Treatments, Appointments)
├── Data/
│   ├── Configurations/   Fluent API configurations and seed data
│   └── Models/           Entity classes and enums
├── Migrations/        Entity Framework Core migrations
├── Services/          Business logic - service interfaces and their implementations
├── ViewModels/        View models used by the views
├── Views/             Razor views, partial views and the shared layout
└── wwwroot/           Static files - CSS, JavaScript, images
```

---

## Author

**Boris Evtimov** - [GitHub](https://github.com/B-Evtimov)

## License

This project is licensed under the MIT License. See [LICENSE.txt](LICENSE.txt) for details.
