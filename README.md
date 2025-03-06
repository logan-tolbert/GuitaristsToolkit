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

 #### 🎵 Practice Tracker

Log practice sessions with date, duration, and focus area (e.g., scales, chords, songs).
View practice history and track progress over time.

 #### 📋 Setlist Manager

Create, edit, and manage setlists for gigs or jam sessions.
Add song details (title, key, BPM, duration, and notes).

#### 🔐 User Authentication

Custom authentication system using cookie-based authentication (no Identity).
Secure user registration and login with BCrypt password hashing.

## 🛠 Tech Stack

- C# Backend logic
- JavaScript for client-side interactivity
- ASP.NET Core MVC Web application with Razor Pages View rendering 
- SQL Server Database
- Dapper ORM for database access
- Bootstrap + CSS UI styling
- BCrypt Password hashing
- xUnit: For unit testing.
- Moq: For creating mock objects in unit tests.

## 🏗 Installation & Setup

**Prerequisites**

- .NET 8.0 SDK
- SQL Server or localdb
- Visual Studio or VS Code

### 📌 Step 1: Clone the Repository

```sh
git clone https://github.com/logan-tolbert/TheGuitaristsToolkit.git
cd TheGuitaristsToolkit
```
### 📌 Step 2: Open the Project in Visual Studio
- Open Visual Studio.
- Click on File > Open > Project/Solution.
- Select the .sln file in the project folder.

### 📌 Step 3: Restore Dependencies
Before running the project, ensure all required dependencies are installed:

- Open the Package Manager Console in Visual Studio (Tools > NuGet Package Manager > Package Manager Console).
- Run the following command:
```powershell
dotnet restore
```
### 📌 Step 4: Build the Project
To build the project, either:

- Click Build > Build Solution from the menu, or
Run the following command in the Terminal or Package Manager Console:
```powershell
dotnet build
```
### 📌 Step 5: Database Setup 

To set up the database using **App.DbDeploy**, follow these steps:

#### 🚀 Step 1: Open the Publish Wizard
1. Open **Visual Studio** and navigate to the **Solution Explorer**.
2. Locate the project containing **App.DbDeploy**.
3. **Right-click** on the project name.
4. Select **Publish** from the context menu.

#### ⚙️ Step 2: Configure the Database Connection
1. In the **Publish** window, enter the **Database Name**.
2. Set up the **Connection String** by selecting or creating a new database.
3. *(Optional)* Save a **Publish Profile** for future use.
4. Click **Publish** to deploy the database.

#### 📜 Step 3: Execute the Deployment Script
Once published, the **post-deployment script** will automatically:
- **Populate the database with initial data**.
- **Create an admin user** with the default password:  `testPassword`

#### 🔑 Step 4: Customizing the Admin Password
If you want to **change the default admin password**, follow these steps:
1. Locate the **post-deployment script** in `App.DpDeploy/scripts` before publishing.
2. Find the section where the **admin user** is created.
3. Replace the default password hash with a new **bcrypt hash**.
4. Save the changes before publishing.

#### 🔗 Step 5: Retrieve and Configure the Connection String
1. After publishing, retrieve the **Connection String** from the database configuration.
2. Create as **`appsettings.json`** file in `App.Web`.
3. Update the **ConnectionStrings** section with your new database connection:
```json
"ConnectionStrings": {
	"Defaulte": "Server=YOUR_SERVER;Database=GuitaristsToolkitTestDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```
### ⚡Step 5: Run the Project
Once the build is successful, you can run the application:

- Click Run (▶️) / Start Debugging (F5) in Visual Studio,
OR
- Use the command:
```powershell
dotnet run
```
This will start the application, and you should see output indicating that your ASP.NET Core server is running.

## Configure Integration Tests

Create a separate `appsettings.Test.json` file in the `App.Tests` project with its own connection string:
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
