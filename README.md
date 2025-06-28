# Technical Service Tracking and Management System

This project is a web-based tracking and management system developed to digitize technical service operations, enhance communication between customers and staff, and provide transparency throughout the entire process. Customers can register their products, create service requests for faulty items, and track the status of these requests in real-time. The system also offers a centralized, role-based management portal for technical service personnel.

## 🏛️ Architecture

The project is designed based on **Onion Architecture**, adhering to modern software principles. This architectural approach places the business logic (Domain) at the core of the application and directs all dependencies toward the outer layers. This provides the following advantages:

* **Sustainability:** The core business rules are completely independent of external factors like the database or user interface.
* **Flexibility:** Future technological changes (e.g., migrating to a different database) can be implemented easily without affecting the application's core.
* **Testability:** Since the business logic is isolated from external dependencies, writing unit tests becomes much easier.

The project is structured into the following layers: `Core`, `Infrastructure`, `Presentation`, and `Shared`.

## ✨ Core Features

### Customer Functions
* **User Management:** Register with email/password, log in, and update profile information.
* **Security:** Email verification, password reset, and secure logout.
* **Product Management:** Register owned products into the system using their serial numbers.
* **Service Management:** Easily create service requests for registered products and track their status (Pending, In-Progress, Completed, etc.) step-by-step.
* **Map Integration:** View technical service locations on a map.

### Personnel Functions (Role-Based)
* **Segregated Portals:** Purpose-driven interfaces segregated for different personnel roles (`Admin`, `Management`, `Operational Staff`).
* **Service Request Management:** List, view details, assign to personnel, and update the status of service requests.
* **Process Steps:** Add new steps to the service process (e.g., "Fault identified," "Waiting for parts").
* **Asset Management (Admin):** Full CRUD (Create, Read, Update, Delete) operations for core assets like Brands, Product Types, Products, and Serial Numbers.
* **Personnel Management (Admin):** Add new personnel to the system, and update their information and roles.
* **Legal Document Management (Admin):** Edit legal texts such as Privacy Policy and Terms of Use.

## 🚀 Technologies & Patterns

### Technology Stack

| Category         | Technology                               |
| ---------------- | ---------------------------------------- |
| **Backend** | .NET 9, ASP.NET Core Web API, C#         |
| **Frontend** | Blazor WebAssembly, MudBlazor            |
| **Database** | Entity Framework Core (Code-First)       |
| **Architecture** | Onion Architecture                       |

### Design Patterns

* **CQRS (Command Query Responsibility Segregation):** The responsibilities for data reading (Query) and writing (Command) are separated using the `MediatR` library. This creates a cleaner, more flexible, and optimized business logic layer.
* **Repository & Unit of Work Pattern:** Data access logic is abstracted from the business logic using the `Repository` pattern. The `Unit of Work` pattern ensures that multiple operations are executed consistently within a single transaction, guaranteeing data integrity.
* **Result Pattern:** The return types of API endpoints and service methods are standardized. This simplifies error handling on the client side.
* **AutoMapper:** Mappings between database entities and Data Transfer Objects (DTOs) are automated.
* **FluentValidation:** The validity of incoming requests is checked with fluent and readable rules before they reach the business logic.

### Security

* **Authentication:** `Cookie-Based Authentication` is used. Cookies are configured as `HttpOnly` and `Secure` to enhance security.
* **Authorization:** `Role-Based Authorization` ensures that each role (Admin, Management, Customer, etc.) can only access authorized API endpoints and pages.
* **Password Security:** User passwords are saved in the database after being hashed with `Argon2id`, a modern and secure hashing algorithm.

## ⚙️ Getting Started

Follow the steps below to run the project on your local machine:

1.  **Clone the Project:**
    ```bash
    git clone https://github.com/kursatabayli/TechnicalService.git
    ```

2.  **Database and API Configuration:**
    * Open the `appsettings.Development.json` (or `appsettings.json`) file in the `Presentation/TechnicalService.WebAPI/` directory.
    * Update the `ConnectionStrings` section with your own database connection details.
    * Fill in the sensitive settings like `JwtSection`, `SmtpSettings`, and `SmsSection` with your service credentials.

3.  **Google Maps API Key:**
    * Open the `Presentation/TechnicalService.UserUI/wwwroot/index.html` file.
    * In the line `https://maps.googleapis.com/maps/api/js?key=API_KEY`, replace `API_KEY` with your own Google Maps API key.
    * Do the same for the `API_KEY` in the `Presentation/TechnicalService.Portal/wwwroot/js/mapInterop.js` file.

4.  **Running the Project:**
    * Open the `TechnicalService.sln` solution in Visual Studio.
    * In the Solution Explorer, right-click the Solution and select `Configure Startup Projects...`.
    * Select the `Multiple startup projects` option and set the `Action` for `TechnicalService.WebAPI`, `TechnicalService.UserUI`, and `TechnicalService.Portal` projects to `Start`.
    * When you run the project, the API, Customer UI, and Personnel Portal will start together.

## 🔮 Future Work

The project's current architecture provides a solid foundation for future enhancements. Potential improvements include:

* **Comprehensive Logging:** Integrate a structured logging library like `Serilog` to monitor system behavior and errors on a centralized platform (e.g., Seq, ELK Stack).
* **Automated Tests:** Write unit tests for the Application layer using `xUnit` or `NUnit`, and integration tests for API endpoints to increase system reliability.
* **Caching:** Implement a caching mechanism like `Redis` for frequently accessed and rarely changing data (e.g., brand lists, product types) to reduce database load and improve API response times.
* **Real-Time Notifications:** Integrate `SignalR` to display real-time updates for service status changes in the UI without requiring a page refresh.
* **Rate Limiting:** Implement request limiting to prevent malicious or excessive use of the API.
* **Enhanced Session Management for Multi-Device Support:** The current architecture stores a single refresh token per user, limiting a user's ability to refresh their session to only one device at a time. This work involves evolving the architecture to a multi-session model by creating a dedicated session table in the database. This table will store a unique refresh token for each active login (per device or browser), enabling users to maintain concurrent sessions across multiple devices and giving them the ability to view and revoke individual sessions remotely.
