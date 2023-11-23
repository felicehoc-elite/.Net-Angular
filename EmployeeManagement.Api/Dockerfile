FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EmployeeManagement.Api/EmployeeManagement.Api.csproj", "EmployeeManagement.Api/"]
COPY ["EmployeeManagement.Core/EmployeeManagement.Core.csproj", "EmployeeManagement.Core/"]
RUN dotnet restore "./EmployeeManagement.Api/./EmployeeManagement.Api.csproj"
COPY . .
WORKDIR "/src/EmployeeManagement.Api"
RUN dotnet build "./EmployeeManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EmployeeManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmployeeManagement.Api.dll"]