# Bigai.Holidays.Core.Infra.Data
----------
Provides support for accessing the data repository using Entity Framework Core.

## Dependências

* [Microsoft.EntityFrameworkCore.SqlServer versão 3.1.8] (https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/3.1.8) - Para acesso ao SQL Server utilizando Entity Framework Core.
* [Microsoft.EntityFrameworkCore.Tools versão 3.1.8] (https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/3.1.8) - Para rodar as migrations de criação do banco de dados.
* [Microsoft.EntityFrameworkCore.Design versão 3.1.8] (https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/3.1.8) - Ferramentas utilizadas pelo Entity Framework Core.
* [Microsoft.Extensions.Configuration versão 3.1.8] (https://www.nuget.org/packages/Microsoft.Extensions.Configuration/3.1.8) - Para obter a configuração do provider.
* [Microsoft.Extensions.Configuration.FileExtensions versão 3.1.8] (https://www.nuget.org/packages/Microsoft.Extensions.Configuration.FileExtensions/3.1.8) - Métodos de extensão de configuration provider.
* [Microsoft.Extensions.Configuration.Json versão 3.1.8] (https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json/3.1.8) - Métodos de extensão de configuration provider.
* [Bigai.Holidays.Shared.Domain] (/src/shared/Bigai.Holidays.Shared.Domain/README.md) - For shared domain access.
* [Bigai.Holidays.Core.Domain] (/src/core/Bigai.Holidays.Core.Domain/README.md) - For access to the core domain.

## Package Installation

```

PM> Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 3.1.8

PM> Install-Package Microsoft.EntityFrameworkCore.Tools -Version 3.1.8

PM> Install-Package Microsoft.EntityFrameworkCore.Design -Version 3.1.8

PM> Install-Package Microsoft.Extensions.Configuration -Version 3.1.8

PM> Install-Package Microsoft.Extensions.Configuration.FileExtensions -Version 3.1.8

PM> Install-Package Microsoft.Extensions.Configuration.Json -Version 3.1.8

```

## Migration

1. Define a project as a Startup Project;

2. Insert the database connection string entries in the Startup project's ```appsettings.json```;

3. Add an entry for the database context at the project startup, in this way, for example;

```

public static IServiceCollection AddContextConfiguration(this IServiceCollection services, IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString(HolidaysContext.KeyConnectionString);
    services.AddDbContext<HolidaysContext>(options =>
    {
        options.UseSqlServer(connectionString);
    });

    return services;
}

```

4. Select the ```Bigai.Holiday.Core.Infra.Data``` project in the Package Manager Console;

5. Define which environment is running the migration, Development, Staging or Production;

```

PM> $env:ASPNETCORE_ENVIRONMENT='Development'

```

6. Create the migration;

```

PM> Add-Migration InitialCreate -Context HolidaysContext -OutputDir Migrations\SqlServer\Holidays

```

7. You can optionally generate the script for creating the database;

```

PM> Script-Migration -Context HolidaysContext

```

8. Create the database;

```

PM> update-database -Context HolidaysContext

PM> update-database -verbose -Context HolidaysContext

```
