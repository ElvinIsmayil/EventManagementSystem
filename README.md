
```
# 📅 EventManagementSystem

## 📝 Overview
**EventManagementSystem** is a **.NET 9.0 ASP.NET Core MVC** web application designed to simplify event management. It provides a comprehensive solution for creating events, managing participants, and handling event logistics with a clean, layered architecture.

---

## ✨ Features
- ✅ Event creation and management
- ✅ Participant registration and tracking
- ✅ Role-based access control (Admin/User)
- ✅ Responsive UI with Bootstrap 5
- ✅ Entity Framework Core with SQL Server
- ✅ Repository pattern implementation
- ✅ Dependency Injection
- ✅ Model-View-ViewModel (MVVM) pattern

---

## 🧩 Architecture
The solution follows a **multi-project architecture**:

```
EventManagementSystem/
├── Data/                # Data layer (Entities, DbContext, Migrations)
│   ├── Entities/        # Domain models
│   ├── Enums/           # Application enums
│   └── DataContext.cs   # DbContext implementation
├── Core/                # Core business logic
│   ├── Interfaces/      # Repository interfaces
│   ├── Services/        # Business services
│   └── ViewModels/      # ViewModels for UI
├── Web/                 # Presentation layer
│   ├── Controllers/     # MVC Controllers
│   ├── Views/           # Razor Views
│   ├── wwwroot/         # Static files
│   └── Program.cs       # Startup configuration
└── Tests/               # (Planned) Test projects
```

**Key Technologies**:
- ASP.NET Core MVC (.NET 9.0)
- Entity Framework Core 9.0
- SQL Server Database
- Bootstrap 5 for responsive UI
- Repository Pattern
- Dependency Injection

---

## 🧑‍💻 Getting Started

### 📋 Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- SQL Server 2019+ (LocalDB included with Visual Studio works)
- Visual Studio 2022 or VS Code with C# extensions

### ⚙️ Installation
1. **Clone the repository**:
   ```bash
   git clone https://github.com/ElvinIsmayil/EventManagementSystem.git
   cd EventManagementSystem
   ```

2. **Configure the database**:
   - Update connection string in `Web/appsettings.json`:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EventManagementDB;Trusted_Connection=True;"
     }
     ```
   - Apply migrations:
     ```bash
     cd Web
     dotnet ef database update
     ```

3. **Run the application**:
   ```bash
   dotnet run --project Web
   ```
   Or from Visual Studio:
   - Set `Web` as startup project
   - Press F5 to run

4. **Browse to** `https://localhost:7279`

---

## 🧪 Testing
The solution currently includes basic functionality testing through the UI. Planned test projects include:
- Unit tests for core business logic
- Integration tests for API endpoints
- UI tests for critical user flows

---

## 🛠️ Development Guidelines

### Code Structure
- **Data Layer**:
  - Entities go in `Data/Entities/`
  - DbContext in `Data/DataContext.cs`
  - Add new migrations via `dotnet ef migrations add [Name]`

- **Core Layer**:
  - Business logic in `Core/Services/`
  - Interfaces in `Core/Interfaces/`
  - ViewModels in `Core/ViewModels/`

- **Presentation Layer**:
  - Controllers in `Web/Controllers/`
  - Views in `Web/Views/`
  - Static assets in `Web/wwwroot/`

### Best Practices
- Follow SOLID principles
- Use async/await for I/O operations
- Keep controllers lean - move logic to services
- Use ViewModels to separate presentation from domain models
- Add XML comments for public methods

---

## 🔐 Security Features
- Input validation on all forms
- Anti-forgery tokens
- Secure cookie policies
- Environment-based configuration
- Role-based authorization

---

## 📜 License
MIT License - See [LICENSE](LICENSE) file for details.

---

## 🤝 Contributing
Contributions are welcome! Please:
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📈 Roadmap
- [ ] Implement authentication/authorization
- [ ] Add event categories and tags
- [ ] Develop reporting features
- [ ] Create API endpoints for mobile access
- [ ] Implement email notifications
- [ ] Add unit and integration tests

---

## 💬 Support
For questions or issues, please [open an issue](https://github.com/ElvinIsmayil/EventManagementSystem/issues) on GitHub.
