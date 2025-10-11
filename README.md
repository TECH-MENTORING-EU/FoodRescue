# FoodRescue

A Blazor web application for managing food donations and rescuing surplus food to help communities in need.

## 🚀 Technology Stack

- **Framework**: Blazor Web App (.NET 9.0)
- **Styling**: Tailwind CSS v3
- **Database**: SQL Server with Dapper ORM
- **Test Data**: Bogus (Faker)
- **Logging**: Microsoft.Extensions.Logging
- **Authentication**: Development-friendly mock authentication
- **Testing**: xUnit, Moq, FluentAssertions
- **Deployment**: IIS-ready with web.config

## 📋 Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js](https://nodejs.org/) (for Tailwind CSS)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB or full installation)
- IDE: Visual Studio 2022, Visual Studio Code, or Rider

## 🛠️ Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/TECH-MENTORING-EU/FoodRescue.git
cd FoodRescue
```

### 2. Restore .NET Dependencies

```bash
dotnet restore
```

### 3. Install Node.js Dependencies and Build Tailwind CSS

```bash
cd src/FoodRescue.Web
npm install
npm run build:css
```

### 4. Set Up the Database

Create the database in SQL Server:

```sql
CREATE DATABASE FoodRescue;
GO

USE FoodRescue;
GO

CREATE TABLE FoodDonations (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DonorName NVARCHAR(200) NOT NULL,
    FoodType NVARCHAR(100) NOT NULL,
    Quantity INT NOT NULL,
    Unit NVARCHAR(50) NOT NULL,
    DonationDate DATETIME2 NOT NULL,
    PickupLocation NVARCHAR(500) NOT NULL,
    IsPickedUp BIT NOT NULL DEFAULT 0
);
GO
```

### 5. Update Connection String

The connection string is configured in `appsettings.json` and `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "FoodRescueDb": "Server=localhost;Database=FoodRescue;Integrated Security=True;TrustServerCertificate=True;"
  }
}
```

Update the connection string according to your SQL Server configuration.

### 6. Run the Application

```bash
cd src/FoodRescue.Web
dotnet run
```

Navigate to `https://localhost:5001` or `http://localhost:5000`

### 7. Run Tests

```bash
cd tests/FoodRescue.Tests
dotnet test
```

## 📁 Project Structure

```
FoodRescue/
├── src/
│   └── FoodRescue.Web/
│       ├── Authentication/          # Authentication handlers
│       ├── Components/              # Blazor components
│       │   ├── Layout/             # Layout components
│       │   └── Pages/              # Page components
│       ├── Models/                 # Data models
│       ├── Repositories/           # Data access layer (Dapper)
│       ├── Services/               # Business logic services
│       ├── Styles/                 # Tailwind CSS source
│       ├── wwwroot/                # Static files
│       ├── appsettings.json        # Configuration
│       └── web.config              # IIS deployment configuration
├── tests/
│   └── FoodRescue.Tests/          # Unit tests
└── FoodRescue.sln                 # Solution file
```

## 🔐 Authentication

The application uses a simple development authentication handler that automatically authenticates users in development mode. This makes it easy to develop and test without complex authentication setup.

### For Development:
- Users are automatically authenticated as "Developer"
- No login required

### For Production:
Replace the `DevAuthenticationHandler` with a proper authentication provider like:
- Azure AD
- ASP.NET Core Identity
- IdentityServer
- OAuth/OpenID Connect

## 🎨 Tailwind CSS

Tailwind CSS is configured and ready to use. The build process is integrated via npm scripts.

### Development Mode (Watch for Changes):
```bash
npm run watch:css
```

### Production Build:
```bash
npm run build:css
```

## 🧪 Testing

The solution includes a comprehensive test project with:
- Unit tests for services and repositories
- Moq for mocking dependencies
- FluentAssertions for readable assertions
- Moq.Dapper for testing Dapper repositories

Run tests with:
```bash
dotnet test
```

## 📦 Deployment to IIS

### 1. Publish the Application

```bash
dotnet publish -c Release -o ./publish
```

### 2. Install ASP.NET Core Hosting Bundle

Download and install the [.NET Hosting Bundle](https://dotnet.microsoft.com/download/dotnet/9.0) on your IIS server.

### 3. Configure IIS

1. Create a new Application Pool with ".NET CLR Version" set to "No Managed Code"
2. Create a new website or application
3. Point the physical path to the publish directory
4. Ensure the Application Pool identity has access to the database

The included `web.config` is pre-configured for IIS deployment with:
- AspNetCoreModuleV2
- InProcess hosting model
- Appropriate request filtering

### 4. Update Connection String

Update the connection string in `appsettings.json` in the publish directory to point to your production database.

## 🔧 Configuration

### Connection Strings

Located in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "FoodRescueDb": "Server=localhost;Database=FoodRescue;Integrated Security=True;TrustServerCertificate=True;"
  }
}
```

### Logging

Logging is configured in `appsettings.json`. Adjust log levels as needed:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## 🧑‍💻 Development

### Using Test Data

The application includes Bogus for generating realistic test data. Navigate to the "Donations" page and click "Generate Sample Data" to see it in action.

### Adding New Features

1. **Models**: Add to `Models/` directory
2. **Repositories**: Create interface and implementation in `Repositories/`
3. **Services**: Add business logic in `Services/`
4. **Pages**: Create Blazor components in `Components/Pages/`
5. **Tests**: Add corresponding tests in the test project

### Tailwind CSS Customization

Edit `tailwind.config.js` to customize your design system:

```javascript
module.exports = {
  theme: {
    extend: {
      colors: {
        // Add custom colors
      },
    },
  },
}
```

## 📝 License

[Specify your license here]

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📧 Contact

For questions or support, please contact the maintainers.