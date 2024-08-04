# Vehicle Registration System Backend

## Overview

This is the backend service for the Vehicle Registration System, developed as part of my personal Service-Oriented Architecture course. The backend is built using ASP.NET Core and serves as the main API for managing vehicle registrations, owners, and insurances. It uses PostgreSQL for database management and provides secure authentication and authorization through the Identity Framework and JWT tokens.

## Technologies Used

- **ASP.NET Core:** The primary framework for building the backend API.
- **Entity Framework Core:** An ORM for database access and management.
- **PostgreSQL:** The relational database for storing application data.
- **Identity Framework:** For managing user identities and roles.
- **JWT Tokens:** For secure, token-based authentication.
- **Xunit:** For unit testing.

## Getting Started

### Prerequisites

- **.NET SDK:** Make sure you have the .NET SDK installed. You can download it from the [official .NET website](https://dotnet.microsoft.com/download).
- **PostgreSQL:** Install PostgreSQL and set up a database. You can download it from the [official PostgreSQL website](https://www.postgresql.org/download/).
- **Visual Studio or VS Code:** For development and running the application.

### Setup Instructions

1. **Clone the Repository:**

   ```bash
   git clone [https://github.com/VullnetA/VehicleRegistrationSystem.git](https://github.com/VullnetA/VehicleRegistrationSystem.git)
   cd VehicleRegistrationSystem

2. **Configure the Database:**
   - Create a PostgreSQL database for the application.
   - Update the connection string in **appsettings.json** with your PostgreSQL database details:

    ```json
   "ConnectionStrings": {
   "DefaultConnection": "Host=localhost;Database=your_db_name;Username=your_username;Password=your_password"
   }
   
3. **Apply Migrations:**
   Run the following command to apply database migrations:
   
   ```bash
   dotnet ef database update

4. **Run the application:**
   Use the following command to run the application:

   ```bash
   dotnet run

5. **Access the API:**
   The API will be available at **http://localhost:5000**

## Usage Guidelines

### API Endpoints

**Vehicle Management**

- **GET /api/Vehicle** - List all vehicles.
- **POST /api/Vehicle** - Register a new vehicle.
- **GET /vehicle/{id}** - Get vehicle by ID.
- **DELETE /vehicle/{id}** - Delete vehicle by ID.
- **PUT /vehicle/{id}** - Update vehicle by ID.
- **GET /vehiclesbyowner/{id}** - List all vehicles by owner.
- **GET /vehiclesbyyear/{year}** - List all vehicles by production year.
- **GET /vehiclesbyfuel/{fuel}** - List all vehicles by fuel type.
- **GET /vehiclesbybrand/{brand}** - List all vehicles by manufacturer.
- **GET /countbybrand/{brand}** - Count vehicles by manufacturer.
- **GET /countbyfueltype/{fuel}** - Count all vehicles by fuel type.
- **GET /countbyyear/{year}** - Count all vehicles by production year.
- **GET /countbycategory/{category}** - Count all vehicles by category.
- **GET /countunregistered** - Count all unregistered vehicles.
- **GET /countregistered** - Count all registered vehicles.
- **GET /counttransmission/{transmission}** - Count all vehicles by transmission.
- **GET /licenseplate/{licensePlate}** - Get vehicle by license plate.
- **GET /checkregistration/{id}** - Check vehicle registration.

**Owner Management**

- **GET /api/Owner** - List all owners.
- **POST /api/Owner** - Register a new owner.
- **GET /owner/{id}** - Get owner by ID.
- **DELETE /owner/{id}** - Delete owner by ID.
- **PUT /owner/{id}** - Update owner by ID.
- **GET /findByCity/{placeOfBirth}** - List all owners by city of birth.
- **GET /findByVehicle/{manufacturer}/{model}** - List owners by vehicle brand.
- **GET /licensesByCity/{placeOfBirth}** - Count licenses by city.
- **GET /searchByName/{name}** - Get owner by name.

**Insurance Management**

- **GET /api/Insurance** - List all insurances.
- **POST /api/Insurance** - Register a new insurance.
- **GET /insurance/{id}** - Get insurance by ID.
- **DELETE /insurance/{id}** - Delete insurance by ID.
- **PUT /insurance/{id}** - Update insurance by ID.
- **GET /insurance/vehicle/{id}** - Get insurance by vehicle ID.
- **GET /count** - Count total number of insurances.
- **GET /expiredinsurances** - Count total number of expired insurances.

### Testing
**Run Tests:**
Execute the following command to run unit tests


    dotnet test

### Contributing
We welcome contributions! If you want to contribute to this project, please follow these steps:

1. Fork the repository.
2. Create a new branch (**'git checkout -b feature-branch'**).
3. Commit your changes (**'git commit -m "commit message"'**).
4. Push to the branch (**'git push origin feature-branch'**).
5. Open a pull request.

### Contact
For any inquiries or support, please contact me at vullnetazizi9@gmail.com.
