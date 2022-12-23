# Buddhabrot API

[![.NET](https://github.com/ebeeton/buddhabrot/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ebeeton/buddhabrot/actions/workflows/dotnet.yml)

![Buddhabrot example](/assets/images/sample.jpg)

This is a hobby ASP.NET Core Web API project for rendering
[Buddhabrot](https://en.wikipedia.org/wiki/Buddhabrot) images in parallel. It's
written in C# and uses Docker to store images to SQL Server and log to Seq. The
sample image was originally 4096x4096 with 200,000,000 sample points and 1,000 
maximum iterations. It took about four minutes to plot on an eight core machine.

The plotting is currently noisy and I'm still tinkering with how to achieve the
really cool images that you see in other people's projects. This is a work in
progress.

## How It Works

The API offers a controller for creating Mandelbrot and Buddhabrot plots and
retrieving images. Because the plotting process is a potentially long-running
operation, plot requests are placed in a database queue and serviced
individually. The plot creation response includes the plot's database ID,
and the image can be retrieved by that ID. If the image plot is complete, a PNG
image will be returned, otherwise it will respond with HTTP 202 Accepted.

Please note that the database is dropped and created every time the app runs.
I haven't yet worked out how to stand up the database and run creation scripts
in a separate service. 

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
