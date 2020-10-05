# Bigai.Holidays.Core.Services.Api
----------
É um microservice, API RESTful, de Feriados.

## Dependências

* [Microsoft.AspNetCore.Mvc.Versioning versão 4.1.1] (https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Versioning/4.1.1) - Para controle de versionamento da Api.
* [Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer verssão 4.1.1] (https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer/4.1.1) - Para expor versionamento da Api.
* [Microsoft.Extensions.PlatformAbstractions -versão 1.1.0] (https://www.nuget.org/packages/Microsoft.Extensions.PlatformAbstractions/1.1.0) - Para documentação com Swagger.
* [Swashbuckle.AspNetCore version 5.6.3] (https://www.nuget.org/packages/Swashbuckle.AspNetCore/5.6.3) - Para documentação da Api.

* [Bigai.Holidays.Core.Infra.CrossCutting.IoC] (/src/Bigai.Holidays.Core.Infra.CrossCutting.IoC/README.md) - Para injeção de dependências.

* [Install-Package Microsoft.AspNetCore.Mvc.Api.Analyzers versão 2.2.0] (Install-Package Microsoft.AspNetCore.Mvc.Api.Analyzers -Version 2.2.0) - Para análise da documentação da Api.
* [MediatR versão 7.0.0] (https://www.nuget.org/packages/MediatR/7.0.0) - Para o envio de Commands.
* [MediatR.Extensions.Microsoft.DependencyInjection versão 7.0.0] (https://www.nuget.org/packages/MediatR.Extensions.Microsoft.DependencyInjection/7.0.0) - Para injeção de dependência do MediatR.
* [Bigai.MangoApi.Identity.Services.Api] (/src/Bigai.MangoApi.Identity.Services.Api/README.md) - Para acesso ao contexto do Identity.

* [Bigai.MangoApi.Cadastros.Infrastructure.Persistence] (/src/Bigai.MangoApi.Cadastros.Infrastructure.Persistence/README.md) - Para acesso ao contexto do banco de de dados de Cadastros.
* [Bigai.MangoApi.Identity.Domain] (/src/Bigai.MangoApi.Shared.Identity.Domain/README.md) - Para acesso ao usuário logado.
* [Bigai.MangoApi.Identity.Services.Api] (/src/Bigai.MangoApi.Shared.Identity.Services.Api/README.md) - Para acesso ao usuário logado.
* [Bigai.MangoApi.Shared.Persistence.EventStore] (/src/Bigai.MangoApi.Shared.Persistence.EventStore/README.md) - Para acesso ao contexto do banco de de dados de Eventos.
* [Bigai.MangoApi.Identity.CrossCutting.IoC] (/src/Bigai.MangoApi.Identity.CrossCutting.IoC/README.md) - Para injeção de dependência do Identity.

## Instalação dos Pacotes

```
PM> Install-Package Microsoft.AspNetCore.Mvc.Versioning -Version 4.1.1

PM> Install-Package Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer -Version 4.1.1

PM> Install-Package Microsoft.Extensions.PlatformAbstractions -Version 1.1.0

PM> Install-Package Swashbuckle.AspNetCore -Version 5.6.3




PM> Install-Package Microsoft.AspNetCore.Mvc.Api.Analyzers -Version 2.2.0

PM> Install-Package MediatR.Extensions.Microsoft.DependencyInjection -Version 7.0.0

```