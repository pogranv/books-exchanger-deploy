FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

ENV ASPNETCORE_ENVIRONMENT=Development

WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5004

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BooksExchanger/BooksExchanger.csproj", "BooksExchanger/"]
RUN dotnet restore "BooksExchanger/BooksExchanger.csproj"
COPY . .
WORKDIR "/src/BooksExchanger"
RUN dotnet build "BooksExchanger.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BooksExchanger.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BooksExchanger.dll"]
