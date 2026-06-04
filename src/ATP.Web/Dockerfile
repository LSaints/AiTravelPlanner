# =========================
# Frontend (Tailwind)
# =========================
FROM node:20 AS frontend

WORKDIR /src

COPY package*.json ./

RUN npm ci

COPY Styles ./Styles
COPY wwwroot ./wwwroot

RUN npm run build


# =========================
# Build .NET
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ATP.Web.csproj ./

RUN dotnet restore

COPY . .

# Copia o CSS gerado pelo Tailwind
COPY --from=frontend /src/wwwroot/css ./wwwroot/css

RUN dotnet publish -c Release -o /app/publish


# =========================
# Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "ATP.Web.dll"]