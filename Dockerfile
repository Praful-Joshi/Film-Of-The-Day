# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy csproj files and restore dependencies first (cache layer)
COPY FilmOfTheDay.Web/FilmOfTheDay.Web.csproj FilmOfTheDay.Web/
COPY FilmOfTheDay.Infrastructure/FilmOfTheDay.Infrastructure.csproj FilmOfTheDay.Infrastructure/
COPY FilmOfTheDay.Core/FilmOfTheDay.Core.csproj FilmOfTheDay.Core/
RUN dotnet restore FilmOfTheDay.Web/FilmOfTheDay.Web.csproj

# Copy everything else and build
COPY . .
RUN dotnet publish FilmOfTheDay.Web/FilmOfTheDay.Web.csproj -c Release -o /app/publish

# Use the ASP.NET runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
# Fly.io requires listening on 8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV DOTNET_ENVIRONMENT=Production
EXPOSE 8080
ENTRYPOINT ["dotnet", "FilmOfTheDay.Web.dll"]
