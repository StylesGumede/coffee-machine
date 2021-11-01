FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/CoffeeMachine/CoffeeMachine.csproj", "CoffeeMachine/"]
RUN dotnet restore "src/CoffeeMachine/CoffeeMachine.csproj"
COPY . .
WORKDIR "/src/CoffeeMachine"
RUN dotnet build "CoffeeMachine.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoffeeMachine.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoffeeMachine.dll"]
