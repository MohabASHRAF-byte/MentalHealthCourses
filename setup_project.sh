dotnet new sln -n MentalHealthcare
mkdir Src
mkdir Tests
cd Src
dotnet new webapi -n MentalHealthcare.API --no-openapi -controllers
cd ..
dotnet sln add Src/MentalHealthcare.API
cd Src
dotnet new classlib -n MentalHealthcare.Domain
cd ..
dotnet sln add Src/MentalHealthcare.Domain
cd Src
dotnet new classlib -n MentalHealthcare.Infrastructure
cd ..
dotnet sln add Src/MentalHealthcare.Infrastructure
cd Src
dotnet new classlib -n MentalHealthcare.Application
cd ..
dotnet sln add Src/MentalHealthcare.Application
cd Src

dotnet add MentalHealthcare.API/MentalHealthcare.API.csproj reference MentalHealthcare.Domain/MentalHealthcare.Domain.csproj

dotnet add MentalHealthcare.API/MentalHealthcare.API.csproj reference MentalHealthcare.Infrastructure/MentalHealthcare.Infrastructure.csproj

dotnet add MentalHealthcare.API/MentalHealthcare.API.csproj reference MentalHealthcare.Application/MentalHealthcare.Application.csproj
echo -e "\033[32mAll References added MentalHealthcare.API.csproj! \033[0m"


dotnet add MentalHealthcare.Application/MentalHealthcare.Application.csproj reference MentalHealthcare.Domain/MentalHealthcare.Domain.csproj
echo -e "\033[32mAll References added MentalHealthcare.Application.csproj! \033[0m"


dotnet add MentalHealthcare.Infrastructure/MentalHealthcare.Infrastructure.csproj reference MentalHealthcare.Domain/MentalHealthcare.Domain.csproj

dotnet add MentalHealthcare.Infrastructure/MentalHealthcare.Infrastructure.csproj reference MentalHealthcare.Application/MentalHealthcare.Application.csproj
echo -e "\033[32mAll References added MentalHealthcare.Infrastructure.csproj! \033[0m"


cd MentalHealthcare.Application
dotnet add MentalHealthcare.Application.csproj package AutoMapper
dotnet add MentalHealthcare.Application.csproj package FluentValidation.AspNetCore
dotnet add MentalHealthcare.Application.csproj package MediatR
dotnet add MentalHealthcare.Application.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add MentalHealthcare.Application.csproj package Microsoft.Extensions.DependencyInjection.Abstractions
dotnet add MentalHealthcare.Application.csproj package Microsoft.Extensions.Logging.Abstractions
cd ..
echo -e "\033[32mAll packages and project references have been added to MentalHealthcare.Application.csproj! \033[0m"


cd MentalHealthcare.Domain
dotnet add MentalHealthcare.Domain.csproj package Microsoft.AspNetCore.Identity.EntityFrameworkCore
cd ..
echo -e "\033[32mAll packages and project references have been added to MentalHealthcare.Domain.csproj! \033[0m"


cd MentalHealthcare.API
dotnet add MentalHealthcare.API.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add MentalHealthcare.API.csproj package Microsoft.EntityFrameworkCore.Tools
dotnet add MentalHealthcare.API.csproj package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add MentalHealthcare.API.csproj package Serilog
dotnet add MentalHealthcare.API.csproj package Serilog.AspNetCore
dotnet add MentalHealthcare.API.csproj package Serilog.Sinks.ApplicationInsights
dotnet add MentalHealthcare.API.csproj package Swashbuckle.AspNetCore
dotnet add MentalHealthcare.API.csproj package Swashbuckle.AspNetCore.Annotations
cd ..
echo -e "\033[32mAll packages and project references have been added to MentalHealthcare.API.csproj! \033[0m"


cd MentalHealthcare.Infrastructure
dotnet add MentalHealthcare.Infrastructure.csproj package Azure.Storage.Blobs
dotnet add MentalHealthcare.Infrastructure.csproj package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add MentalHealthcare.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add MentalHealthcare.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer
dotnet add MentalHealthcare.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools
dotnet add MentalHealthcare.Infrastructure.csproj package Npgsql.EntityFrameworkCore.PostgreSQL
cd ..
echo -e "\033[32mAll packages and project references have been added to MentalHealthcare.Infrastructure.csproj! \033[0m"

