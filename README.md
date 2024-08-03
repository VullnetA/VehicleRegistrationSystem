# Vehicle Registration System Backend

## Overview

This is the backend service for the Vehicle Registration System, developed as part of a Service-Oriented Architecture course. The backend is built using ASP.NET Core and serves as the main API for managing vehicle registrations, owners, and insurances. It uses PostgreSQL for database management and provides secure authentication and authorization through the Identity Framework and JWT tokens.

## Technologies Used

- **ASP.NET Core:** The primary framework for building the backend API.
- **Entity Framework Core:** An ORM for database access and management.
- **PostgreSQL:** The relational database for storing application data.
- **Identity Framework:** For managing user identities and roles.
- **JWT Tokens:** For secure, token-based authentication.
- **Xunit:** For unit testing.
- **Service-Repository Pattern:** For separating business logic from data access.
- **Caching:** For improving performance and reducing database load.

## Getting Started

### Prerequisites

- **.NET SDK:** Make sure you have the .NET SDK installed. You can download it from the [official .NET website](https://dotnet.microsoft.com/download).
- **PostgreSQL:** Install PostgreSQL and set up a database. You can download it from the [official PostgreSQL website](https://www.postgresql.org/download/).
- **Visual Studio or VS Code:** For development and running the application.

### Setup Instructions

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/yourusername/vehicle-registration-backend.git
   cd vehicle-registration-backend
