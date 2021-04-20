FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["MSMensajes.csproj", "./"]
RUN dotnet restore "MSMensajes.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "MSMensajes.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MSMensajes.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MSMensajes.dll"]