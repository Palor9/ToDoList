﻿FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ToDoList.csproj", "./"]
RUN dotnet restore "ToDoList.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "ToDoList.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ToDoList.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDoList.dll"]
