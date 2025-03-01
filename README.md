# EventPlatformProjectMVC  

## Overview  
EventPlatformProjectMVC is a powerful and user-friendly ASP.NET Core MVC application designed to streamline event management. It enables organizers to create and manage events effortlessly while allowing participants to browse, register, and engage with events seamlessly. With a clean and responsive design, the platform ensures an optimal user experience across various devices.  

## Key Features  

- **Event Creation & Management** – Organizers can create, edit, and delete events with essential details such as title, description, date, time, and location.  
- **User Authentication & Authorization** – Secure user registration, login, and role-based access control using ASP.NET Identity.  
- **Event Registration** – Participants can explore available events and register quickly.  
- **Secure Payment Integration** – Stripe payment gateway is integrated to handle online transactions for paid events.  
- **Responsive & Modern UI** – The application adapts to different screen sizes, ensuring a smooth experience on desktops, tablets, and mobile devices.  
- **Scalable & Maintainable Architecture** – Built using a clean architecture approach for better maintainability and scalability.  

## Technology Stack  

### Backend  
- **ASP.NET Core MVC** – Provides a robust and scalable framework for web applications.  
- **Entity Framework Core** – ORM for seamless database interactions.  
- **SQL Server** – Relational database for storing event and user-related data.  
- **ASP.NET Identity** – Secure authentication and user management system.  
- **Stripe Payment Gateway** – Facilitates secure and efficient payment processing.  

### Design Patterns & Architecture  
- **Clean Architecture** – Structured into multiple layers for better separation of concerns:  
  - **UI Layer** – Handles presentation logic.  
  - **Application Layer** – Contains business logic and use cases.  
  - **Infrastructure Layer** – Manages database interactions, external services, and third-party integrations.  
  - **Domain Layer** – Defines core business entities and rules.  
- **Unit of Work Pattern** – Ensures efficient database transactions.  
- **Repository Pattern** – Provides a structured way to interact with the data layer.  
- **Dependency Injection** – Enables loose coupling and better testability.  

## Getting Started  

### Prerequisites  
Ensure you have the following installed on your system:  
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet)  
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)  
- Visual Studio or VS Code for development  

### Installation Steps  

1. **Clone the repository**:  
   ```bash  
   git clone https://github.com/ahmedgamal23/EventPlatformProjectMVC.git  
   cd EventPlatformProjectMVC  
   ```  

2. **Configure the Database**:  
   - Update the connection string in `appsettings.json` to match your SQL Server instance.  
   - Run the following command to apply migrations:  
     ```bash  
     dotnet ef database update  
     ```  

3. **Run the Application**:  
   ```bash  
   dotnet run  
   ```  

4. **Access the Web Application**:  
   - Open your browser and navigate to `http://localhost:5000` (or the configured port).  

## Contributing  
Contributions are welcome! To contribute:  
1. Fork the repository.  
2. Create a new branch (`feature-branch-name`).  
3. Commit your changes.  
4. Open a pull request for review.  


