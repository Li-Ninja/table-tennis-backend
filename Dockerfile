FROM mcr.microsoft.com/dotnet/sdk:8.0.100-rc.2.23502.2 AS build
WORKDIR /src
COPY ["table-tennis-backend.csproj", "./"]
RUN dotnet restore "table-tennis-backend.csproj"
COPY . .
RUN dotnet publish "table-tennis-backend.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0.100-rc.2.23502.2
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "table-tennis-backend.dll"]
