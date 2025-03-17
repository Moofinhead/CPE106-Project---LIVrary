# Library Management System

## Overview

The Library Management System is a comprehensive WPF application designed to streamline library operations. This system provides an intuitive interface for both administrators and users to manage book circulation, user accounts, and library resources efficiently.

## Features

### Administrator Features
- **Book Management**: Add, update, and remove books from the library catalog
- **User Management**: Create and manage user accounts with different access levels
- **Request Processing**: Review and approve book borrowing requests
- **Return Processing**: Process book returns and manage late fees
- **Transaction History**: View complete history of book circulation

### User Features
- **Book Catalog**: Browse and search the library's collection
- **Book Requests**: Request books for borrowing
- **Account Management**: View borrowed books and transaction history
- **Deadline Notifications**: Receive warnings about approaching return deadlines

## Technical Architecture

The application follows a three-tier architecture:

1. **Presentation Layer** (WPF UI)
   - XAML-based user interface with responsive design
   - Separate views for admin and user operations

2. **Business Logic Layer** (BL)
   - Implements core business rules and validation
   - Manages data flow between UI and data access layer

3. **Data Access Layer** (DAL)
   - Handles database operations and data persistence
   - Provides abstraction for database interactions

## Getting Started

### Prerequisites
- Windows OS
- .NET Framework 4.7.2 or later
- SQL Server (for database)

### Installation
1. Clone the repository
2. Open the solution in Visual Studio
3. Build the solution to restore NuGet packages
4. Update the connection string in `App.config` to point to your database
5. Run the application

## Usage

### Admin Login
- Username: admin
- Password: admin123

### Sample User
- Username: johndoe@gmail.com
- Password: password
- Name: John Doe

## Project Structure
Library-Management-System/
├── LibraryManagementSystem/ # Main WPF Application
├── LibraryMSWF.BL/ # Business Logic Layer
├── LibraryMSWF.DAL/ # Data Access Layer
└── LibraryMSWF.Entity/ # Entity Classes

## Technologies Used

- C# / .NET Framework
- Windows Presentation Foundation (WPF)
- MVVM Architecture Pattern
- SQL Server Database
- Entity Framework
