# Todo API

A simple ASP.NET Core Web API for managing TODO items, featuring a layered architecture, Entity Framework Core migrations, and Docker Compose support.

### Features

* DTOs and object mapping
* Repository and Service layers
* `TodoDbContext` with EF Core (PostgreSQL)
* Docker Compose setup for API and PostgreSQL

### Prerequisites

* .NET 8 SDK or later
* Docker & Docker Compose (v2.x)

### Getting Started

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
