# BookBridge
BookBridge is an ASP.NET Core Web API designed to facilitate book sharing within a community. Users can register, list books they are willing to share, and request books from others.

# Table of Contents
- Features
- Installation
- Usage
- Endpoints
- Contributing
- License
- Features
- User registration and authentication
- Book listing and management (CRUD operations)
- Book borrowing and returning
- User-to-user messaging
- Search functionality for books and users
- Installation
- Prerequisites
- .NET 8 SDK
- SQL Server
- Steps

Clone the repository:

```sh
git clone https://github.com/RaisaBadal/BookBridge.git
```
```sh
cd BookBridge
```

# Set up the database:

- Update the appsettings.json file with your SQL Server connection string.

#Run the following command to apply migrations:

```sh
dotnet ef database update
```

# Run the application:
```sh
dotnet run
```
# Running the API
To start the API, run the following command:

```sh
dotnet run
``` 
- The API will be available at https://localhost:5001 (or http://localhost:5000).

# Swagger UI
- To explore and test the API endpoints, navigate to https://localhost:5001/swagger in your browser.

# Endpoints
# Users
- POST /api/users/register - Register a new user
- POST /api/users/login - Authenticate a user
- GET /api/users - Get all users
- GET /api/users/{id} - Get a specific user by ID
- PUT /api/users/{id} - Update a user
- DELETE /api/users/{id} - Delete a user
# Books
- GET /api/books - Get all books
- GET /api/books/{id} - Get a specific book by ID
- POST /api/books - Create a new book
- PUT /api/books/{id} - Update a book
- DELETE /api/books/{id} - Delete a book
# Borrowing
- POST /api/borrow - Request to borrow a book
- PUT /api/borrow/{id}/return - Return a borrowed book
# Messaging
- GET /api/messages - Get all messages
- GET /api/messages/{id} - Get a specific message by ID
- POST /api/messages - Send a new message
# Contributing
```sh
Contributions are welcome! Please fork this repository and submit pull requests with your changes. Ensure that your code adheres to the existing coding standards and includes appropriate tests.
```

# Fork the repository
- Create a new branch (git checkout -b feature/your-feature)
- Commit your changes (git commit -m 'Add some feature')
- Push to the branch (git push origin feature/your-feature)
-Open a pull request
# License
```sh
This project is licensed under the MIT License. See the LICENSE file for details.
```

Feel free to modify this template to better suit the specific details and needs of your project.
