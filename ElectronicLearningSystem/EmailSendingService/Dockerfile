# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем только нужные проекты и их зависимости
COPY ./EmailSendingService ./EmailSendingService/
COPY ./ElectronicLearningSystemCore ./ElectronicLearningSystemCore/
COPY ./ElectronicLearningSystemKafka ./ElectronicLearningSystemKafka/


# Восстанавливаем зависимости
RUN dotnet restore ./EmailSendingService/EmailSendingService.csproj
RUN dotnet restore ./ElectronicLearningSystemKafka/ElectronicLearningSystemKafka.csproj
RUN dotnet restore ./ElectronicLearningSystemCore/ElectronicLearningSystemCore.csproj

# Сборка проекта
WORKDIR /src/EmailSendingService
RUN dotnet publish -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app

# Копируем собранный проект из build-стадии
COPY --from=build /app/out .

# Указываем команду запуска
ENTRYPOINT ["dotnet", "EmailSendingService.dll"]