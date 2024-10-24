# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todo el contenido
COPY . .

# Restaurar dependencias
RUN dotnet restore "appAPI.csproj"

# Publicar la aplicación
RUN dotnet publish "appAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Configurar variables de entorno y puerto
ENV ASPNETCORE_URLS=http://+:5050  
ENV SUPABASE_URL=https://ceahusezvwdlyrkfinol.supabase.co
ENV SUPABASE_KEY=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImNlYWh1c2V6dndkbHlya2Zpbm9sIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTcyOTUxNzI0NywiZXhwIjoyMDQ1MDkzMjQ3fQ.-Lty422itbXwaO0MQjTadsdigR3v5uJsBnGobsdi4EM

EXPOSE 5050  

# Iniciar la aplicación
ENTRYPOINT ["dotnet", "appAPI.dll"]
