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
- **Soft delete for barbers** - inactive barbers are hidden from the public pages without losing their appointment history
- **Seeded database** - the application starts with ready-to-use barbers, categories, services and barber-service relations

### Planned

- Booking an appointment with server-side and client-side validation
- Full CRUD for appointments (create, view, edit, cancel)
- Protection against double-booking the same barber at the same time
- Service layer with dependency injection for clean separation of concerns
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
├── Controllers/       MVC controllers (Barbers, Treatments, Home)
├── Data/
│   ├── Configurations/   Fluent API configurations and seed data
│   └── Models/           Entity classes and enums
├── Migrations/        Entity Framework Core migrations
├── ViewModels/        View models used by the views
├── Views/             Razor views, partial views and the shared layout
└── wwwroot/           Static files - CSS, JavaScript, images
```

---

## Author

**Boris Evtimov** - [GitHub](https://github.com/B-Evtimov)

## License

This project is licensed under the MIT License. See [LICENSE.txt](LICENSE.txt) for details.