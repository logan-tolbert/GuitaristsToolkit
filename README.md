# 🎸 Guitarist's Toolkit

Guitarist's Toolkit is a web application designed for musicians to track their practice sessions and manage setlists for performances. This tool helps guitarists develop structured practice habits and organize songs for gigs or jam sessions.

## Table of Contents
- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Installation & Setup](#-installation--setup)
- [Authentication & Authorization](#-authentication--authorization)
- [Project Structure](#-project-structure)
- [Contributing](#-contributing)
- [License](#-license)

## 🚀 Features

### 🎵 Practice Tracker

Log practice sessions with date, duration, and focus area (e.g., scales, chords, songs).
View practice history and track progress over time.

### 📋 Setlist Manager

Create, edit, and manage setlists for gigs or jam sessions.
Add song details (title, key, BPM, duration, and notes).

### 🔐 User Authentication

Custom authentication system using cookie-based authentication (no Identity).
Secure user registration and login with BCrypt password hashing.

## 🛠️ Tech Stack

- C# Backend logic
- JavaScript for client-side interactivity
- ASP.NET Core MVC Web application with Razor Pages View rendering 
- SQL Server Database
- Dapper ORM for database access
- Bootstrap + CSS UI styling
- BCrypt Password hashing
- xUnit: For unit testing.
- Moq: For creating mock objects in unit tests.

## 🏗️ Installation & Setup

### 1️⃣ Prerequisites

.NET 8.0 SDK
SQL Server or localdb
Visual Studio or VS Code

### 2️⃣ Clone the Repository

```sh
git clone https://github.com/logan-tolbert/TheGuitaristsToolkit.git
cd TheGuitaristsToolkit
```

### 3️⃣ Configure Database

Modify appsettings.json to include your database connection string:

```json
"ConnectionStrings": {
	"Default": "Server=YOUR_SERVER;Database=GuitaristsToolkitDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 4️⃣ Apply Migrations

```sh
dotnet ef database update
```

### 5️⃣ Run the Application

```sh
dotnet run
```

Then navigate to http://localhost:5000 in your browser.

### 6️⃣ Configure Integration Tests

Create a separate `appsettings.Test.json` file in the `IntegrationTests` folder with its own connection string:
```json
"ConnectionStrings": {
	"Testing": "Server=YOUR_SERVER;Database=GuitaristsToolkitTestDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

## 🔑 Authentication & Authorization

User authentication is handled via cookie-based authentication.
Practice sessions and setlists are tied to the logged-in user.

## 📂 Project Structure

```plaintext
/Controllers - Handles HTTP requests
/Models - Defines data models
/Repo - Database access layer
/Views - Razor pages for UI
/wwwroot - Static assets (CSS, JS)
```

## 👥 Contributing

Feel free to submit pull requests and report issues!

## 📜 License

This project is licensed under the MIT License.
