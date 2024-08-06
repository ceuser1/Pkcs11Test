#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# CUSTOM ADD - add environment variables
ENV PKCS11_LOGGER_LIBRARY_PATH=/pkcs11/libcs_pkcs11_R2.so
ENV PKCS11_LOGGER_LOG_FILE_PATH=/pkcs11/logger.log
ENV PKCS11_LOGGER_FLAGS=0

RUN mkdir /pkcs11/
RUN chmod 777 /pkcs11
COPY ["Files/libcs_pkcs11_R2.so", "/pkcs11/libcs_pkcs11_R2.so"]
COPY ["Files/cs_pkcs11_R2_docker_linux.cfg", "/pkcs11/cs_pkcs11_R2.cfg"]
COPY ["Files/pkcs11-logger-x64.so", "/pkcs11/pkcs11-logger-x64.so"]
ENV CS_PKCS11_R2_CFG="/pkcs11/cs_pkcs11_R2.cfg"
# CUSTOM END

USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Pkcs11Test/Pkcs11Test.csproj", "Pkcs11Test/"]
RUN dotnet restore "./Pkcs11Test/Pkcs11Test.csproj"
COPY . .
WORKDIR "/src/Pkcs11Test"
RUN dotnet build "./Pkcs11Test.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Pkcs11Test.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pkcs11Test.dll"]