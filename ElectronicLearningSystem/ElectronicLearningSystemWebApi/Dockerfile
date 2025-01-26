# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем проекты
COPY ./ElectronicLearningSystemWebApi ./ElectronicLearningSystemWebApi/
COPY ./ElectronicLearningSystemCore ./ElectronicLearningSystemCore/
COPY ./ElectronicLearningSystemKafka ./ElectronicLearningSystemKafka/

# Восстанавливаем зависимости
RUN dotnet restore ./ElectronicLearningSystemKafka/ElectronicLearningSystemKafka.csproj
RUN dotnet restore ./ElectronicLearningSystemCore/ElectronicLearningSystemCore.csproj
RUN dotnet restore ./ElectronicLearningSystemWebApi/ElectronicLearningSystemWebApi.csproj

# Сборка проекта
WORKDIR /src/ElectronicLearningSystemWebApi
RUN dotnet publish -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Копируем собранный проект из build-стадии
COPY --from=build /app/out .

# Указываем команду запуска
ENTRYPOINT ["dotnet", "ElectronicLearningSystemWebApi.dll"]
EXPOSE 5000
