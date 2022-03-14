FROM node:16.13.2-alpine AS frontend-build
WORKDIR /app
COPY ./Frontend .
RUN npm ci
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:6.0 as backend-build
WORKDIR /backend
COPY ./Backend .
RUN dotnet restore
RUN dotnet build -o /app -c Release

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=backend-build /app .
COPY --from=frontend-build /app/dist/email-management ./wwwroot
ENTRYPOINT ["dotnet", "Email.Management.dll"]