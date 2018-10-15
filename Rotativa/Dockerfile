FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
RUN apt-get update -qq && apt-get -y install libgdiplus libc6-dev
EXPOSE 80

FROM microsoft/dotnet:2.1.400-sdk AS build
WORKDIR /src
COPY src .
WORKDIR /src/Rotativa
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Rotativa.dll"]