# Hospital Appointment Management System

A full-stack hospital appointment management web application built with ASP.NET Core MVC, Entity Framework Core, and MySQL.

The system provides functionality for managing patients, doctors, departments, doctor schedules, and appointments through a structured service-based architecture.

## Features

- User registration and login
- Session-based user management
- Department management
- Doctor management
- Doctor session and availability management
- Patient appointment creation
- Department and doctor selection during appointment booking
- Doctor session selection
- Appointment management
- Administrative dashboard
- CRUD operations for departments, doctors, and doctor sessions
- MySQL database integration
- Entity Framework Core migrations
- Dependency Injection and service-layer architecture

## Technology Stack

### Backend

- C#
- ASP.NET Core MVC
- .NET 10
- Entity Framework Core 10
- MySQL

### Frontend

- Razor Views
- HTML
- CSS
- Bootstrap
- JavaScript
- jQuery

### Architecture & Tools

- MVC Architecture
- Service Layer
- Dependency Injection
- Session Management
- Entity Framework Core Migrations
- Git

## Project Structure

```text
hospital-appointment-system/
├── Controllers/
│   ├── AccountController.cs
│   ├── AdminController.cs
│   ├── DepartmentsController.cs
│   ├── DoctorsController.cs
│   ├── DoctorSessionsController.cs
│   ├── PatientAppointmentsController.cs
│   └── HomeController.cs
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── Models/
│   ├── Appointment.cs
│   ├── Department.cs
│   ├── Doctor.cs
│   ├── DoctorSession.cs
│   ├── Patient.cs
│   ├── Payment.cs
│   └── User.cs
│
├── Services/
│   ├── AppointmentService.cs
│   ├── DepartmentService.cs
│   ├── DoctorService.cs
│   ├── DoctorSessionService.cs
│   ├── AdminDashboardService.cs
│   └── Service Interfaces
│
├── Views/
│   ├── Account/
│   ├── Admin/
│   ├── Departments/
│   ├── Doctors/
│   ├── DoctorSessions/
│   ├── PatientAppointments/
│   ├── Home/
│   └── Shared/
│
├── Migrations/
├── wwwroot/
├── Program.cs
├── appsettings.example.json
└── BeykentHospitalAppointment.csproj
```

## Application Flow

A patient can:

```text
Register / Login
       |
       v
Select Department
       |
       v
Select Doctor
       |
       v
Select Available Session
       |
       v
Create Appointment
       |
       v
View Appointment
```

Administrative functionality allows hospital-related data such as departments, doctors, and doctor sessions to be managed through dedicated interfaces.

## Database

The application uses MySQL with Entity Framework Core.

Database entities include:

- Users
- Patients
- Doctors
- Departments
- Doctor Sessions
- Appointments
- Payments

Entity Framework Core migrations are included in the repository for database schema management.

## Configuration

The real `appsettings.json` file is excluded from Git to prevent database credentials from being published.

Create your local configuration based on:

```text
appsettings.example.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1;Port=3306;Database=YOUR_DATABASE_NAME;User=YOUR_USERNAME;Password=YOUR_PASSWORD;"
  }
}
```

Replace the placeholder values with your local MySQL configuration.

## Installation

Clone the repository:

```bash
git clone https://github.com/edaakgc/hospital-appointment-system.git
cd hospital-appointment-system
```

Restore the required packages:

```bash
dotnet restore
```

Configure the database connection in your local `appsettings.json`.

Apply the Entity Framework migrations:

```bash
dotnet ef database update
```

Run the application:

```bash
dotnet run
```

Then open the local address displayed in the terminal.

## Architecture

The application follows the MVC pattern and separates business logic through a dedicated service layer.

```text
HTTP Request
     |
     v
Controller
     |
     v
Service Layer
     |
     v
Entity Framework Core
     |
     v
MySQL Database
```

Service interfaces and Dependency Injection are used to reduce coupling between controllers and business logic.

## Security & Configuration

- Database credentials are excluded from version control.
- A safe `appsettings.example.json` file is provided for configuration.
- Session cookies are configured as HTTP-only.
- HTTPS redirection is enabled.
- Session timeout is configured for 30 minutes.

## Project Objective

The objective of this project is to demonstrate the development of a database-driven web application using ASP.NET Core MVC and Entity Framework Core.

The project focuses on implementing a structured appointment workflow while applying MVC architecture, relational database design, service-layer separation, Dependency Injection, session management, and CRUD operations.

## Disclaimer

This project was developed for educational and portfolio purposes. The application and included data are intended for demonstration and testing and are not designed for use with real patient or medical information.
