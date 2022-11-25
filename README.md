# Buddhabrot API

An ASP.NET Core Web API for rendering [Buddhabrot](https://en.wikipedia.org/wiki/Buddhabrot)
images in parallel. This is a personal programming project inspired by the work of Melinda Green.

An example image with 64 passes:

![Buddhabrot example](/assets/images/sample.jpg)

## Database Configuration

No default password is configured for the SQL Server container; you'll need to
set one, and also include it in the API project's connection string.

### SQL Server

The SQL Server container is configured to read the `sa` user's password from the
`MSSQL_SA_PWD` environment variable. You can create an .env file in the solution
directory to set your password:

```shell
MSSQL_SA_PWD=MySecretPassword
```

### Buddhabrot API

The API project's connection string is configured to connect to the SQL Server
container, but doesn't include the password. You can add your password to the
`DefaultConnection` string in appsettings.json, or set a user secret called
`DbPassword` that will be read at startup.

```shell
dotnet user-secrets set "DbPassword" "MySecretPassword"
```
