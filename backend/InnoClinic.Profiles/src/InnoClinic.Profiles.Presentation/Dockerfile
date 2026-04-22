FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/InnoClinic.Profiles.Presentation/InnoClinic.Profiles.Presentation.csproj", "src/InnoClinic.Profiles.Presentation/"]
RUN dotnet restore "src/InnoClinic.Profiles.Presentation/InnoClinic.Profiles.Presentation.csproj"
COPY . .
WORKDIR "/src/src/InnoClinic.Profiles.Presentation"
RUN dotnet build "./InnoClinic.Profiles.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InnoClinic.Profiles.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InnoClinic.Profiles.Presentation.dll"]
