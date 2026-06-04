FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY . .

# Node
RUN apt-get update && apt-get install -y nodejs npm

# Dependências JS
RUN npm install

# Gera CSS
RUN npm run build

# Publica .NET
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ATP.Web.dll"]