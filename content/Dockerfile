# Stage 1 - Restoring & Compiling
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine as builder
WORKDIR /source
RUN apk add --update nodejs npm
COPY *.csproj .
RUN dotnet restore
COPY package.json .
RUN npm install
COPY . .
RUN dotnet publish -c Release -o /app/

# Stage 2 - Creating Image for compiled app
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine as baseimage
RUN apk add --update nodejs npm
WORKDIR /app
COPY --from=builder /app .
ENV ASPNETCORE_URLS=http://+:$port

# Run the application. REPLACE the name of dll with the name of the dll produced by your application
EXPOSE $port
CMD ["dotnet", "FurnitureBuildingSolution.dll"]
