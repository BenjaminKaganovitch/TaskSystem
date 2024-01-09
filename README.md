# Support Ticket Management System - Part 2

## Overview
This project is an extension of the initial Support Ticket Management System, incorporating Entity Framework and ASP.NET Core Identity. It enhances the system with user registration, authentication, authorization, and advanced data management capabilities.

## Features

### Entity Framework Integration
- **Data Migration:** Migrates seeded data to a database using Entity Framework, leveraging its ORM capabilities for efficient data handling.
- **CRUD Operations:** The application supports all CRUD operations (Create, Read, Update, Delete) using Entity Framework, allowing for robust data management.
- **TicketController Enhancement:** Updated to utilize Entity Framework for efficient database interactions.

### ASP.NET Core Identity
- **User Authentication System:** Implements a comprehensive user authentication system including registration, login, and logout functionalities.
- **User-Ticket Association:** Each ticket is associated with the user account that created it, ensuring data integrity and security.

### User Roles and Authorization
- **Role-Based Access Control:** Implements "Admin" and "User" roles, providing a layered access control system.
- **Authorization Mechanism:** Admins can manage all tickets in the system, while regular users have access only to tickets they created.

### User Profile
- **Personalized User Dashboard:** A dedicated user profile page displaying user details and a list of their tickets, enhancing user experience and system navigation.

## Technologies Used
- ASP.NET Core Identity for authentication and authorization.
- Entity Framework for database operations and management.
- SQL
- Azure Data Management Studio

---

*This README provides an overview of the Support Ticket Management System - Part 2, designed to demonstrate advanced capabilities in web application development using ASP.NET Core and Entity Framework.*
