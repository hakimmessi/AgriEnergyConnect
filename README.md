# AgriEnergyConnect

# Development Environment Setup
Prerequisites

Must have: 
-	Visual Studio 2022 with .NET 8.0
-	SQL Server (Express Edition is sufficient)

Step-by-Step Instructions: 

1. Install .NET SDK
	Download and install .NET 8.0 SDK from the official.

2. Install SQL Server
	Download and install SQL Server 

3. Install Visual Studio
	Download and install Visual Studio 2022
	During installation, ensure to include the following workloads:
o	ASP.NET and web development
o	.NET desktop development

4. Clone the Repository:
	Open a terminal or Git Bash and clone the repository: https://github.com/hakimmessi/AgriEnergyConnect.git

	If you don't use Git, you can download the project as a ZIP file and extract it to your desired location.



# Building and Running the Prototype. 

Step-by-Step Instructions

1.	Open the Project in Visual Studio
	Open Visual Studio.
	Click on "Open a project or solution" and navigate to the project directory, then open the `.sln` file.

2.	Configure Database Connection Strings
	Open `appsettings.json` in the `AgriEnergyConnect.Web` project.
	Update the connection strings to match your local SQL Server instance:  
```json
     {
       "ConnectionStrings": {
        "DefaultConnection": "Server=your_server_name;Database=IdentityDb;User Id=your_user;Password=your_password;",
         "AgriEnergyConnectDbConnection": "Server=your_server_name;Database=AgriEnergyConnectDataDB;User Id=your_user;Password=your_password;"
}
}
 ```

3.	Update Database Migrations
	Open the Package Manager Console (PMC) in Visual Studio (Tools > NuGet Package Manager > Package Manager Console).
	Run the following commands to update the databases:
     ```sh
    Update-Database -Context ApplicationDbContext
    Update-Database -Context AgriEnergyConnectDbContext
   ```
4.	Run the Application
	Press `F5` or click on the "Start Debugging" button in Visual Studio to build and run the application.

 # Database Configuration. 

Restore Databases

	Install SQL Server and SSMS on the new computer (if not already installed).
	Open SSMS, right-click on `Databases`, and select `Restore Database`
	Choose `Device`, browse to the `.bak` files, and restore the databases.

Configure Connection Strings

	Open the `appsettings.json` file in the project on the new computer.
	   Update the connection strings to match the new SQL Server instance.
