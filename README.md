# Fit Flow Users API

This is the users microservice for the Fit Flow application, built with .NET 8. It handles user management, goals, and workout routines.

## Features

- User management (create and retrieve users)
- Goal management and assignment
- Integration with workout routines service
- Redis caching, event handling and used as a database

## Tech Stack

- .NET 8
- Entity Framework Core (SQLite)
- Redis for caching/messaging
- Docker support
- Swagger/OpenAPI documentation

## Prerequisites

- .NET 8 SDK
- Redis server
- Docker (optional)

## Configuration

The application uses the following configuration:

- Redis connection string (in appsettings.json)
- Routines service URL

## Running Locally

1. Clone the repository:
```sh
git clone https://github.com/yourusername/fit-flow.git
cd fit-flow
```

2. Start Redis server locally or use a remote instance

3. Update connection strings in appsettings.json

4. Run the application:
```sh
cd fit-flow-users.WebApi
dotnet run
```

The API will be available at `https://localhost:5001`

## Docker Deployment

1. Build the Docker image:
```sh
docker build -t fit-flow-users .
```

2. Run the container:
```sh
docker run -p 8080:80 fit-flow-users
```

## API Endpoints

- `GET /api/status` - Check API status
- `POST /api/users` - Create new user
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `DELETE /api/users/{id}` - Delete user

## Project Structure

- `/Controllers` - API endpoints
- `/Services` - Business logic
- `/Models` - Domain models
- `/DTOs` - Data transfer objects
- `/Mappings` - Change from diferent data structures
