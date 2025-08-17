FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY fit-flow-users.WebApi/fit-flow-users.WebApi.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT [ "dotnet", "fit-flow-users.WebApi.dll" ]