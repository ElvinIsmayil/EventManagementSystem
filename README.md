```
markdown
# 📅 EventManagementSystem

## 📝 Overview
**EventManagementSystem** is a **.NET 9.0 ASP.NET Core MVC** web application designed to simplify the process of creating and managing events.  
It allows organizers to create events, manage participant invitations, track attendance, and perform check-ins — all with a clear and structured architecture.

---

## ✨ Features
- ✅ Event creation, update, and cancellation
- ✅ Participant invitation and status tracking
- ✅ Event check-in system
- ✅ Role-based access control
- ✅ AutoMapper-powered DTO mappings
- ✅ SQL Server database with Entity Framework Core
- ✅ Configurable per-environment setup

---

## 🧩 Architecture
This repository follows a **layered architecture**:

```

EventManagementSystem/
├── ClientUI/     # ASP.NET Core MVC front-end (Views, Controllers, wwwroot)
├── ServerUI/     # ASP.NET Core MVC admin panel (Admin dashboard, management UI)
├── BLL/          # Business Logic Layer (DTOs, Services, AutoMapper config)
├── DAL/          # Data Access Layer (Entities, DbContext)
├── .vs/          # Visual Studio config/cache (ignored in version control)

````

**Technologies & Design Patterns**:
- ASP.NET Core MVC (.NET 9.0)
- Entity Framework Core (SQL Server)
- AutoMapper for DTO mapping
- Separation of concerns across ClientUI, ServerUI, BLL, and DAL

---

## 🧑‍💻 Getting Started

### 📋 Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)
- Visual Studio 2022 or VS Code recommended

### ⚙️ Installation
1. **Clone the repository**:
   ```bash
   git clone https://github.com/ElvinIsmayil/EventManagementSystem.git
   cd EventManagementSystem
````

2. **Configure the database connection**
   Edit `ClientUI/appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=EventManagementSystemDB;Trusted_Connection=True;TrustServerCertificate=True"
   }
   ```
3. **Restore and build**:

   ```bash
   dotnet restore
   dotnet build
   ```
4. **Run the application**:

   ```bash
   dotnet run --project EventManagementSystem.ClientUI
   ```
5. **Browse to** `http://localhost:5132`

---

## 🧪 Testing

> ⚠️ **No unit or integration tests yet.**
> Future versions will add automated testing with xUnit/NUnit.

---

## 🛠️ Development Guidelines

* Follow **C# naming conventions** (`PascalCase` for types, `camelCase` for variables).
* Implement new features in the appropriate layer:

  * **DAL** — entities and EF Core logic
  * **BLL** — business services and AutoMapper profiles
  * **ClientUI** — MVC controllers, views, and UI for end-users
  * **ServerUI** — MVC controllers, views, and UI for the admin panel
* Avoid business logic in controllers; delegate to services.

---

## 🧰 Configuration

* `appsettings.json`: Base configuration (connection strings, logging, etc.)
* `appsettings.Development.json`: Development-only settings
* `launchSettings.json`: Run profiles and URLs

---

## 🔐 Security

* Enable HTTPS in production
* Never commit secrets or credentials
* Implement authentication and authorization before public deployment

---

## 📜 License

This project is licensed under the [MIT License](LICENSE).

---

## 🤝 Contributing

Contributions are welcome!
Please:

1. Fork the repo
2. Create a feature branch (`feature/my-feature`)
3. Commit your changes
4. Submit a pull request

---

## 📈 Future Plans

* Implement **Feedback & Notification System**
* Add **Hub Logic**
* Improve **UI/UX** and client-side enhancements
* Set up **QR Codes** for automated check-in

---

**💬 Need help?**
Feel free to open an issue or discussion in the repository.

---

> ✏️ **Last updated:** 22 June 2025

```


