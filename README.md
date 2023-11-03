# cloud-sales .NET Core Web API Project

Integration with Cloud Computing Provider for automation of Cloud Sales

This project is a .NET Core Web API using Entity Framework for data management.

## Prerequisites

Before you begin, ensure you have the following installed on your system:

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Entity Framework Core CLI Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

## Getting Started

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/your-username/your-repo.git
   cd your-repo
   ```

2. **Restore Packages:**

   ```bash
   dotnet restore
   ```

3. **Set Up Database:**

   - Run EF migrations to create the database schema:

   ```bash
   dotnet ef database update
   ```

4. **Run the API:**

   ```bash
   dotnet run
   ```

   The API will be accessible to test it with swagger at `https://localhost:7232` (or `http://localhost:5024`).

## Contributing

Feel free to contribute by opening issues or submitting pull requests.

## License

This project is licensed under the [MIT License](LICENSE).

---
