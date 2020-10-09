# Bigai.Holidays.Core.Services.Api
----------
It is a microservice, RESTful API, for Holidays.

## Dependências

* [Bigai.Holidays.Core.Infra.Data] (/src/core/Bigai.Holidays.Core.Infra.Data/README.md) - For database context access.
* [Bigai.Holidays.Core.Infra.CrossCutting.IoC] (/src/core/Bigai.Holidays.Core.Infra.CrossCutting.IoC/README.md) - To inject application dependencies.
* [Microsoft.AspNetCore.Mvc.Versioning version 4.1.1] (https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Versioning/4.1.1) - For version control of Api.
* [Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer version 4.1.1] (https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer/4.1.1) - For version control of Api.
* [Swashbuckle.AspNetCore version 5.6.3] (https://www.nuget.org/packages/Swashbuckle.AspNetCore/5.6.3) - For generating and displaying documentation using Swagger.
* [Microsoft.Extensions.PlatformAbstractions version 1.1.0] (https://www.nuget.org/packages/Microsoft.Extensions.PlatformAbstractions/1.1.0) - For access to the XML file with controller documentation, by Swagger.
* [AspNetCore.HealthChecks.SqlServer version 3.1.1] (https://www.nuget.org/packages/AspNetCore.HealthChecks.SqlServer/3.1.1) - To check the health of the database.
* [AspNetCore.HealthChecks.UI version 3.1.3] (https://www.nuget.org/packages/AspNetCore.HealthChecks.UI/3.1.3) - To user interface of health check.
* [AspNetCore.HealthChecks.UI.Client version 3.1.2] (https://www.nuget.org/packages/AspNetCore.HealthChecks.UI.Client/3.1.2) - To user interface of health check.

* [Install-Package Microsoft.AspNetCore.Mvc.Api.Analyzers versão 2.2.0] (Install-Package Microsoft.AspNetCore.Mvc.Api.Analyzers -Version 2.2.0) - Para análise da documentação da Api.
* [MediatR versão 7.0.0] (https://www.nuget.org/packages/MediatR/7.0.0) - Para o envio de Commands.
* [MediatR.Extensions.Microsoft.DependencyInjection versão 7.0.0] (https://www.nuget.org/packages/MediatR.Extensions.Microsoft.DependencyInjection/7.0.0) - Para injeção de dependência do MediatR.

## Installing the Packages

```

PM> Install-Package Microsoft.AspNetCore.Mvc.Versioning -Version 4.1.1

PM> Install-Package Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer -Version 4.1.1

PM> Install-Package Swashbuckle.AspNetCore -Version 5.6.3

PM> Install-Package Microsoft.Extensions.PlatformAbstractions -Version 1.1.0

PM> Install-Package AspNetCore.HealthChecks.SqlServer -Version 3.1.1

PM> Install-Package AspNetCore.HealthChecks.UI -Version 3.1.3

PM> Install-Package AspNetCore.HealthChecks.UI.Client -Version 3.1.2



PM> Install-Package Microsoft.AspNetCore.Mvc.Api.Analyzers -Version 2.2.0

PM> Install-Package MediatR.Extensions.Microsoft.DependencyInjection -Version 7.0.0

```