# TaskManagement API

## Setup Instructions

### Prerequisites

Before running the project locally, ensure you have the following tools installed:

- [.NET 6 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Entity Framework Core CLI](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

### Running the Project Locally

1. **Clone the repository:**

    ```bash
    git clone https://github.com/yourusername/TaskManagement.git
    cd TaskManagement
    ```

2. **Run the application:**

    ```bash
    dotnet run
    ```

---

## Architecture and Design Choices

### Architecture Overview

The **TaskManagement API** follows a layered architecture pattern to promote separation of concerns:

- **API Layer**:  
  Exposes RESTful endpoints and handles HTTP requests and responses. Controllers are responsible for interacting with the service layer.

- **Service Layer**:  
  Contains business logic. Services interact with repositories and apply necessary transformations or validations.

- **Repository Layer**:  
  Implements data access logic using Entity Framework Core and interacts with the database.

- **Data Models and DTOs**:  
  Data models represent database entities, while DTOs (Data Transfer Objects) are used to transfer data between the layers without exposing the internal models.

---

### Key Design Patterns

- **Unit of Work and Repository Pattern**:  
  These patterns abstract the data access logic and allow for a more flexible and testable codebase. The `IUnitOfWork` interface ensures that changes are committed in a transactional manner.

- **DTO (Data Transfer Objects)**:  
  DTOs are used to ensure that the API does not expose internal data models. This keeps the internal data structure hidden and reduces the risk of misuse of sensitive data.

- **AutoMapper**:  
  AutoMapper is used to map between domain models and DTOs, simplifying code and ensuring a clean separation between layers.

- **Serilog for Logging**:  
  Serilog is integrated to log application activity, providing insights into system behavior and capturing important events such as exceptions and requests.

- **JWT Authentication**:  
  The system uses JWT tokens for secure authentication and ensures that each task is associated with the authenticated user. This provides secure access to resources.
