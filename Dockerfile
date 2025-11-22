# Etapa 1: Construcción (Build)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar la solución y los archivos de proyecto para restaurar dependencias
COPY *.sln .
COPY Lab10-MunozHerrera-Api/*.csproj ./Lab10-MunozHerrera-Api/
COPY Lab10-MunozHerrera.Application/*.csproj ./Lab10-MunozHerrera.Application/
COPY Lab10-MunozHerrera.Domain/*.csproj ./Lab10-MunozHerrera.Domain/
COPY Lab10-MunozHerrera.Infrastructure/*.csproj ./Lab10-MunozHerrera.Infrastructure/

# Restaurar las dependencias de NuGet
RUN dotnet restore

# Copiar el resto del código fuente
COPY . .

# Publicar la aplicación en modo Release
WORKDIR /app/Lab10-MunozHerrera-Api
RUN dotnet publish -c Release -o /app/out

# Etapa 2: Ejecución (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Configuración de puerto para Render (Render usa la variable PORT dinámicamente)
# Forzamos el puerto 8080 como predeterminado
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Punto de entrada de la aplicación
ENTRYPOINT ["dotnet", "Lab10-MunozHerrera-Api.dll"]