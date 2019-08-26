FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
RUN apt-get update -qq && apt-get -y install libgdiplus libc6-dev
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["Rotativa/Rotativa.csproj", "src/Rotativa/"]
RUN dotnet restore "src/Rotativa/Rotativa.csproj"
COPY . .
WORKDIR "/src/Rotativa"
RUN dotnet build "Rotativa.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Rotativa.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
RUN chmod 755 /app/Rotativa/Linux/wkhtmltopdf
ENTRYPOINT ["dotnet", "Rotativa.dll"]