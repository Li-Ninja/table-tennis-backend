FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["table-tennis-backend.csproj", "./"]
RUN dotnet restore "table-tennis-backend.csproj"
COPY . .
RUN dotnet publish "table-tennis-backend.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "table-tennis-backend.dll"]
