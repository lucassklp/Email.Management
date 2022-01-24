FROM node:16.13.2-alpine AS build-frontend
WORKDIR /app-frontend
COPY ./Frontend .
RUN npm ci
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:6.0 as backend-build
WORKDIR /backend
COPY ./Backend .
COPY --from=build-frontend /app-frontend/dist/email-management ./wwwroot
RUN ls
RUN dotnet restore
RUN dotnet build -o /app -c Release

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=backend-build /app .
ENTRYPOINT ["dotnet", "Email.Management.dll"]