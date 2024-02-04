#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/jiraF.Member.API/jiraF.Member.API.csproj", "src/jiraF.Member.API/"]
RUN dotnet restore "src/jiraF.Member.API/jiraF.Member.API.csproj"
COPY . .
WORKDIR "/src/src/jiraF.Member.API"
RUN dotnet build "jiraF.Member.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "jiraF.Member.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "jiraF.Member.API.dll"]