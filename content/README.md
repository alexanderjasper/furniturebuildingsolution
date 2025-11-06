# Furniture Building Solution

ASP.NET Core & Vue.js Application for custom furniture design and ordering.

---

# Table of Contents

* [Features](#features)
* [Prerequisites](#prerequisites)
* [Installation](#installation)
* [Getting Started](#getting-started)
* [Development](#development)
* [Troubleshooting](#troubleshooting)
* [License](#license)

# Features

- **ASP.NET Core 9.0**
  - Web API
  - JWT Authentication
  - Entity Framework Core
  - Serilog Logging
- **VueJS 2**
  - Vuex (State Store)
  - Vue Router
  - Three.js for 3D visualization
- **Webpack 4**
  - Development and Production builds
  - Hot Module Replacement (HMR)
- **Bootstrap 4 & Bootstrap Vue**

# Prerequisites

 * [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
 * [Node.js](https://nodejs.org/) >= 14.x (LTS recommended)
 * [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or full version)
 * IDE: [Visual Studio 2022](https://visualstudio.microsoft.com/), [Rider](https://www.jetbrains.com/rider/), or [VSCode](https://code.visualstudio.com/)

# Installation

## 1. Clone the Repository

```bash
git clone <repository-url>
cd furniturebuildingsolution
```

## 2. Restore .NET Dependencies

```bash
cd content
dotnet restore
```

## 3. Install Node.js Dependencies

```bash
npm install
```

## 4. Configure Database

Update the connection string in `content/appsettings.json` to point to your SQL Server instance:

```json
{
  "DatabaseSettings": {
    "Server": "localhost",
    "Database": "FurnitureBuildingDB",
    "UserId": "your_user",
    "Password": "your_password"
  }
}
```

For development with SQL Server LocalDB, you can use:
```
Server=(localdb)\\mssqllocaldb;Database=FurnitureBuildingDB;Trusted_Connection=True;
```

## 5. Run Database Migrations

```bash
cd content
dotnet ef database update
```

# Getting Started

## Build Frontend Assets

Before running the application for the first time, you need to build the frontend assets:

### Development Build

```bash
cd content
npm run build-vendor:dev
npx webpack
```

### Production Build

```bash
cd content
npm run build
```

## Start the Application

### Option 1: Using the Command Line

```bash
cd content
dotnet run
```

The application will start on `http://localhost:5001`

### Option 2: Using IDE

**Visual Studio / Rider:**
- Open `FurnitureBuildingSolution.sln`
- Press `F5` to start debugging

**VSCode:**
- Open the `content` folder
- Press `F5` to start debugging (uses `launchSettings.json`)

## View Your Application

Browse to [http://localhost:5001](http://localhost:5001)

**Note:** The application runs on HTTP port 5001 in development mode. HTTPS redirection is disabled in development.

# Development

## Frontend Development

The frontend uses Webpack for bundling. During development:

1. **Watch Mode** (for continuous rebuilds):
   ```bash
   npx webpack --watch
   ```

2. **Development Server** (with HMR - if configured):
   ```bash
   npm run dev
   ```

## Project Structure

```
content/
├── ClientApp/           # Vue.js frontend application
│   ├── components/      # Vue components
│   ├── _services/       # API service layer
│   ├── router/          # Vue Router configuration
│   └── store/           # Vuex state management
├── Controllers/         # ASP.NET Core API controllers
├── Services/            # Business logic layer
├── Repositories/        # Data access layer
├── Database/            # EF Core DbContext
├── wwwroot/            # Static files and built assets
│   └── dist/           # Webpack output
├── Properties/         # Launch settings
└── Views/              # Razor views

```

## Environment Configuration

The application uses `ASPNETCORE_ENVIRONMENT` to determine the environment:

- **Development**: `ASPNETCORE_ENVIRONMENT=Development`
  - HTTP only (no HTTPS redirection)
  - Detailed error pages
  - API URL: `http://localhost:5001`

- **Production**: `ASPNETCORE_ENVIRONMENT=Production`
  - HTTPS redirection enabled
  - Production error handling
  - API URL configured in webpack

Set the environment variable in:
- `Properties/launchSettings.json` (for IDE debugging)
- Command line: `export ASPNETCORE_ENVIRONMENT=Development` (Mac/Linux) or `set ASPNETCORE_ENVIRONMENT=Development` (Windows)

## Database Migrations

To create a new migration:

```bash
cd content
dotnet ef migrations add MigrationName
dotnet ef database update
```

# Troubleshooting

## Port Already in Use

If you get "address already in use" error:

```bash
# Find process using port 5001
lsof -i :5001

# Kill the process
kill -9 <PID>
```

## Frontend Assets Not Loading (404 errors)

If you see 404 errors for `main.js`, `vendor.css`, etc.:

1. Build the frontend assets:
   ```bash
   cd content
   npm run build-vendor:dev
   npx webpack
   ```

2. Hard refresh your browser: `Cmd+Shift+R` (Mac) or `Ctrl+Shift+R` (Windows)

## API Calls Failing

If the frontend can't reach the API:

1. Verify the application is running on `http://localhost:5001`
2. Check the API URL in the browser console
3. Rebuild frontend with correct environment:
   ```bash
   # For development (uses localhost)
   npx webpack
   
   # For production
   npm run build:prod
   ```

## Database Connection Issues

1. Verify SQL Server is running
2. Check connection string in `appsettings.json`
3. Ensure database exists: `dotnet ef database update`

## Styling Not Appearing

If the page loads but has no styling:

1. Ensure `site.css` is built:
   ```bash
   npm run build-vendor:dev
   ```
2. Hard refresh browser to clear cache
3. Check browser console for CSS loading errors

# Recommended Tools

- **Vue.js DevTools**: [Chrome Extension](https://chrome.google.com/webstore/detail/vuejs-devtools/nhdogjmejiglipccpnnnanhbledajbpd)
- **SQL Server Management Studio** (SSMS) for database management
- **.NET CLI**: Included with .NET SDK for command-line operations

# Recommended plugin for debugging VueJS

- Get Chrome DevTools for VueJS [here](https://chrome.google.com/webstore/detail/vuejs-devtools/nhdogjmejiglipccpnnnanhbledajbpd)

----

# Found a Bug? Want to Contribute?

Nothing's ever perfect, but please let me know by creating an issue (make sure there isn't an existing one about it already), and we'll try and work out a fix for it! If you have any good ideas, or want to contribute, feel free to either make an Issue with the Proposal, or just make a PR from your Fork.
Please note that this project is released with a [Contributor Covenant Code of Conduct](CODE_OF_CONDUCT.md). By participating in this project you agree to abide by its terms.

----

# License

[![MIT License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat)](/LICENSE)

Copyright (c) 2016-2019 [Mark Pieszak](https://github.com/MarkPieszak)

[![Twitter Follow](https://img.shields.io/twitter/follow/MarkPieszak.svg?style=social)](https://twitter.com/MarkPieszak)

----

# Trilon - Vue, Asp.NET, NodeJS - Consulting | Training | Development

Check out **[Trilon.io](https://Trilon.io)** for more info! Twitter [@Trilon_io](http://www.twitter.com/Trilon_io)

Contact us at <hello@trilon.io>, and let's talk about your projects needs.

<p align="center">
  <a href="https://trilon.io" target="_blank">
        <img src="https://trilon.io/meta/og-image.png" alt="Trilon.io - Angular Universal, NestJS, JavaScript Application Consulting Development and Training">
  </a>
</p>

## Follow Trilon online:

Twitter: [@Trilon_io](http://twitter.com/Trilon_io)
