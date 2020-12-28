FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY MessagingService.sln .
COPY MessagingService/MessagingService.csproj ./MessagingService/
RUN dotnet restore

# copy everything else and build app
COPY MessagingService/. ./MessagingService/
WORKDIR /source/MessagingService
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./ 
ENTRYPOINT ["dotnet", "MessagingService.dll"]