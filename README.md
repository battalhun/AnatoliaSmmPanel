# AnatoliaSmmPanel

A full-featured **SMM (Social Media Marketing) Panel** built with ASP.NET Core MVC — developed as part of the MCPD Full Stack .NET Developer program.

The project includes both an **admin panel** and a **customer-facing interface**, integrating with a third-party SMM API for order management, service listing, and ticket support.

---

## 🚀 Features

### 👤 Customer Panel
- User registration and login (ASP.NET Core Identity)
- Browse and order SMM services
- Real-time order status tracking
- Support ticket system (open / reply / close)
- Balance and transaction history

### 🛠️ Admin Panel
- Dashboard with statistics
- Service management (add, edit, categorize, bulk operations)
- Order management and status updates
- User management
- Ticket management
- SVG logo with CSS variable color support (via `GetLogoViewComponent`)

### 🔗 SMM API Integration
- Third-party SMM API service layer (`ISmmApiService`)
- Order placement, status check, cancel operations
- Polymorphic JSON response handling (`JsonElement`)
- Enum deserialization with `System.Text.Json`

---

## 🧰 Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend | ASP.NET Core MVC (.NET 8) |
| ORM | Entity Framework Core |
| Database | Microsoft SQL Server |
| Auth | ASP.NET Core Identity |
| Frontend | Bootstrap 5, JavaScript |
| Views | Razor Pages / Partial Views |
| API | RESTful HTTP client (`HttpClient`) |
| Architecture | MVC + Service Layer + DTO pattern |

---

## 📁 Project Structure

```
AnatoliaSmmPanel/
├── Controllers/
│   ├── Admin/           # Admin area controllers (partial classes)
│   └── Customer/        # Customer-facing controllers
├── Models/
│   ├── Entities/        # EF Core entity models
│   ├── DTOs/            # API request/response DTOs
│   └── ViewModels/      # View-specific models
├── Services/
│   └── ISmmApiService   # SMM API abstraction
├── ViewComponents/
│   └── GetLogoViewComponent  # Inline SVG logo
├── Views/
│   ├── Admin/
│   └── Customer/
├── Data/
│   └── ApplicationDbContext  # EF Core context
└── wwwroot/             # Static assets
```

---

## ⚙️ Setup

### Prerequisites
- .NET 8 SDK
- SQL Server (local or remote)
- Visual Studio 2022 / VS Code

### 1. Clone the repository
```bash
git clone https://github.com/battalhun/AnatoliaSmmPanel.git
cd AnatoliaSmmPanel
```

### 2. Configure the database
Update `appsettings.json` with your connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_CONNECTION_STRING_HERE"
  },
  "SmmApi": {
    "BaseUrl": "YOUR_SMM_API_URL",
    "ApiKey": "YOUR_API_KEY"
  }
}
```

### 3. Run migrations
```bash
dotnet ef database update
```

### 4. Run the application
```bash
dotnet run
```

---

## 📌 Key Technical Decisions

- **Partial classes** used for `HomeController` to split admin logic across files
- **GUID → int** user ID mapping via `UserManager<ApplicationUser>.Users`
- **`JsonElement`** used for polymorphic SMM API cancel response
- **Bootstrap 5 dropdown-submenu** implemented for nested admin navbar
- **SVG logo** rendered inline via `@Html.Raw()` to support CSS variable theming

---

## 👤 Developer

**Süleyman Canbaz**
ASP.NET Core Developer | MCPD
[LinkedIn](https://www.linkedin.com/in/suleymancanbaz) · [GitHub](https://github.com/battalhun)
