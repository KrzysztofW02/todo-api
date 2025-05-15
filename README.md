# Todo API

A simple ASP.NET Core 8 Web API for managing TODO items, featuring a clean, layered architecture, Entity Framework Core migrations, Docker Compose orchestration, comprehensive testing, and performance benchmarking.

## ⚙️ Architecture

This solution consists of four projects, each with a clear responsibility:

* **docker-compose**
  Contains `docker-compose.yml` to launch the API service and a PostgreSQL database.

* **todo-api**
  The core ASP.NET Core 8 Web API project, featuring:

  * **Controllers**, **DTOs**, and **Mappings**
  * **Models** and **Data** (`TodoDbContext` with EF Core migrations)
  * **Repositories** and **Services** for business logic

* **TodoApi.Benchmarks**
  BenchmarkDotNet-based project to measure and compare `TodoService` performance with realistic payloads.

* **TodoApi.Tests**
  xUnit project with:

  * **Unit Tests** for service logic (using Moq)
  * **Integration Tests** for controllers (using in-memory EF Core)

## 🛠 Prerequisites

* .NET 8 SDK or later
* Docker & Docker Compose (v2.x)

## 🚀 Getting Started

1. **Clone the repository**

   ```bash
   git clone https://github.com/KrzysztofW02/todo-api.git
   cd todo-api
   ```

2. **Run with Docker Compose**

   ```bash
   docker compose up --build
   ```

   * API will be available at `http://localhost:5000/swagger`
   * PostgreSQL at `localhost:5432`
  
3. **Run Locally (Without Docker)**

   If you have PostgreSQL installed and a `todo_db` database created with matching credentials in `appsettings.json`:
   ```bash
   cd TodoApi
   dotnet build

   dotnet run --project TodoApi
   ```

   * API (HTTPS): `https://localhost:7148/swagger`
   * API (HTTP):  `http://localhost:5217/swagger`
   
   *These ports match the `applicationUrl` settings in `Properties/launchSettings.json`.*
  
## 🧪 Running Tests

All test projects use xUnit.

* **Unit & Integration Tests**

  ```bash
  cd TodoApi.Tests
  dotnet test
  ```

* **Performance Benchmarks**

  ```bash
  cd TodoApi.Benchmarks
  dotnet run -c Release
  ```

Benchmarks use [BenchmarkDotNet](https://benchmarkdotnet.org/) to measure service performance.
