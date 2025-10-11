# Setup Instructions

This document provides quick setup instructions for developers to get started with the FoodRescue application.

## Prerequisites

- .NET 9.0 SDK
- Node.js (v18 or later)
- SQL Server (LocalDB, Express, or full installation)
- An IDE (Visual Studio 2022, VS Code, or Rider)

## Quick Start

### 1. Clone and Navigate

```bash
git clone https://github.com/TECH-MENTORING-EU/FoodRescue.git
cd FoodRescue
```

### 2. Restore .NET Dependencies

```bash
dotnet restore
```

### 3. Install Node Dependencies and Build CSS

```bash
cd src/FoodRescue.Web
npm install
npm run build:css
cd ../..
```

**Important**: The Tailwind CSS must be built before running the application. The generated CSS file (`wwwroot/css/app.css`) is excluded from source control and needs to be generated locally.

### 4. Set Up the Database

Run the SQL script in `database/setup.sql` using SQL Server Management Studio or the command line:

```bash
sqlcmd -S localhost -d master -i database/setup.sql
```

Or manually execute the script in your SQL Server tool.

### 5. Update Connection String (if needed)

The default connection string in `src/FoodRescue.Web/appsettings.Development.json` is:

```json
"Server=localhost;Database=FoodRescue;Integrated Security=True;TrustServerCertificate=True;"
```

Update this if your SQL Server configuration is different.

### 6. Run the Application

```bash
cd src/FoodRescue.Web
dotnet run
```

The application will start on `http://localhost:5277` (or similar). Open this in your browser.

### 7. Run Tests

```bash
cd tests/FoodRescue.Tests
dotnet test
```

## Development Workflow

### Watch Mode for Tailwind CSS

When developing, you can run Tailwind in watch mode to automatically rebuild CSS when you make changes:

```bash
cd src/FoodRescue.Web
npm run watch:css
```

Keep this running in a separate terminal while you develop.

### Authentication

The application uses a development authentication handler that automatically authenticates you as "Developer" in development mode. No login is required.

### Generating Test Data

Navigate to the "Donations" page and click "Generate Sample Data" to see Bogus in action generating realistic test data.

## Troubleshooting

### CSS Not Loading

If the styles aren't loading:
1. Ensure you ran `npm install` in `src/FoodRescue.Web`
2. Run `npm run build:css` to generate the CSS file
3. Check that `wwwroot/css/app.css` exists

### Database Connection Errors

If you get database connection errors:
1. Verify SQL Server is running
2. Check the connection string in `appsettings.Development.json`
3. Ensure the database was created using the setup script
4. Test your connection string with SQL Server Management Studio or Azure Data Studio

### Port Already in Use

If the default port is already in use, update the port in `src/FoodRescue.Web/Properties/launchSettings.json`.

## Additional Resources

See the main [README.md](README.md) for complete documentation.
