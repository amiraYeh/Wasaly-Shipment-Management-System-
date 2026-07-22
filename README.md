# 🚚 Wasaly - Delivery Management System

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-blue)
![C#](https://img.shields.io/badge/C%23-12.0-green)
![SQL Server](https://img.shields.io/badge/SQL_Server-Database-red)
![Entity Framework Core](https://img.shields.io/badge/EF_Core-8.0-purple)
![Status](https://img.shields.io/badge/Status-Completed-success)

A comprehensive delivery management platform built with ASP.NET Core, designed to connect merchants, couriers, and administrators through a centralized logistics system.

---

# 📌 Overview

Wasaly is a full-stack delivery management system that streamlines shipment creation, courier assignment, delivery tracking, and verification processes.

The platform provides dedicated dashboards for administrators, merchants, and couriers while ensuring secure authentication, shipment visibility, and efficient delivery workflows.

---

# ✨ Key Features

## 🔐 Authentication & Authorization
- ASP.NET Core Identity authentication
- Role-Based Access Control (RBAC)
- Secure login and registration

## 📦 Shipment Management
- Create and manage shipments
- Shipment status tracking
- Delivery history
- Assignment management

## 🛵 Courier Management
- Courier registration and verification
- National ID verification
- Driving license verification
- Profile management

## 🏪 Merchant Management
- Merchant dashboard
- Shipment creation
- Store information management
- Balance tracking

## 👨‍💼 Admin Dashboard
- User management
- Courier approval workflow
- Shipment oversight
- System monitoring

## 📍 Location Services
- Google Maps integration
- Pickup and delivery locations
- Route support

## 📧 Notifications
- Gmail SMTP integration
- Delivery notifications
- Account verification emails

## 🔑 Delivery Verification
- OTP-based delivery confirmation
- Secure package handoff process

---

# 🏗️ System Architecture

```text
Presentation Layer (PL)
        │
        ▼
Business Logic Layer (BLL)
        │
        ▼
Data Access Layer (DAL)
        │
        ▼
SQL Server Database
```
### Architecture Layers

#### Presentation Layer (PL)
- Razor Pages UI
- User interaction
- Form validation

#### Business Logic Layer (BLL)
- Business rules
- Services
- Application workflows

#### Data Access Layer (DAL)
- Entity Framework Core
- Database operations
- Repository access

---

# 🔄 Business Workflow

1. Merchant creates a shipment.
2. Shipment enters the system.
3. Admin oversees courier verification.
4. Shipment gets assigned to a courier.
5. Courier updates shipment status.
6. Customer receives OTP.
7. Courier delivers package.
8. OTP verification confirms successful delivery.

---

# 📂 Project Structure

```text
Wasaly
│
├── Wasaly.PL
│   ├── Areas
│   ├── Controllers
│   ├── Views
│   └── wwwroot
│
├── Wasaly.BLL
│   ├── Services
│   ├── Interfaces
│   └── Business Logic
│
└── Wasaly.DAL
    ├── Models
    ├── Context
    └── Data Access
```

---

# 🛠️ Technology Stack

| Category | Technology |
|-----------|------------|
| Framework | ASP.NET Core 8 |
| Language | C# 12 |
| Database | SQL Server |
| ORM | Entity Framework Core 8 |
| Authentication | ASP.NET Core Identity |
| Email Service | MailKit |
| Maps | Google Maps API |
| Frontend | Bootstrap 5 |
| Validation | jQuery Validation |

---

# 🚀 Getting Started

## Prerequisites

- .NET 8 SDK
- SQL Server
- Visual Studio 2022
- Google Maps API Key
- Gmail SMTP Account

---

## Clone Repository

```bash
git clone https://github.com/amiraYeh/Wasaly-Shipment-Management-System-.git
cd Wasaly
```

---

## Configure Database

Update:

```json
appsettings.json
```

with your SQL Server connection string.

---

## Configure Email Service

Configure Gmail SMTP settings inside:

```json
appsettings.json
```

---

## Configure Google Maps

```json
{
  "GoogleMaps": {
    "Key": "YOUR_API_KEY"
  }
}
```

---

## Apply Migrations

```bash
cd Wasaly.PL
dotnet ef database update --project ../Wasaly.DAL
```

---

## Run Application

```bash
dotnet run
```

Navigate to:

```text
https://localhost:5001
```

---

# 👥 User Roles

## Admin

- Manage users
- Verify couriers
- Monitor shipments

## Merchant

- Create shipments
- View shipment history
- Track deliveries
- Manage store information

## Courier

- View assignments
- Update shipment status
- Verify deliveries with OTP
- Track earnings

---

# 🗄️ Core Database Entities

- WasalyIdentityUser
- Merchant
- Courier
- Shipment
- CourierAssignment
- Location
- DeliveryOTP
- ShipmentTracking

---

# 💡 Skills Demonstrated

- ASP.NET Core Development
- Entity Framework Core
- SQL Server Database Design
- Authentication & Authorization
- Role-Based Access Control (RBAC)
- Dependency Injection
- LINQ Queries
- Async Programming
- API Integration
- Email Service Integration
- Google Maps Integration

---

# ⭐ Project Highlights

- Implemented a multi-role authentication system using ASP.NET Core Identity.
- Designed a three-layer architecture (PL, BLL, DAL).
- Built shipment assignment and tracking modules.
- Integrated Google Maps API for location services.
- Implemented OTP-based delivery verification.
- Developed courier document verification workflow.
- Applied Dependency Injection .

---

# 🔒 Security Features

- ASP.NET Core Identity Authentication
- Role-Based Authorization
- OTP Delivery Verification
- Courier Document Verification
- Secure Password Hashing

---

# 🔮 Future Enhancements

- SignalR Real-Time Notifications
- Payment Gateway Integration
- Advanced Analytics Dashboard
- Automated Courier Assignment
- Multi-Language Support

---

# 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit changes
4. Push changes
5. Open a Pull Request

---

# 👩‍💻team members 

**Asmaa Ibrahim**
**Merna Sobhi**
**Remonda Nady**
**Amira Yehia**


---

# 📄 License

Private project — all rights reserved.


---

⭐ If you found this project useful, consider giving it a star on GitHub.
