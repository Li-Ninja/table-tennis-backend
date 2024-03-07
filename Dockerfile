FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["table-tennis-backend.csproj", "./"]
RUN dotnet restore "table-tennis-backend.csproj"
COPY . .
RUN dotnet publish "table-tennis-backend.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=GCP

ARG DB_IP
ARG DB_USER
ARG DB_PASSWORD
ARG YET_ANOTHER_VAR

# 设置多个环境变量
ENV DB_IP=$DB_IP \
    DB_USER=$DB_USER \
    DB_PASSWORD=$DB_PASSWORD

ENTRYPOINT ["dotnet", "table-tennis-backend.dll"]
