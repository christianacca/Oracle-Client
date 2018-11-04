FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Oracle-Client.csproj ./
RUN dotnet restore /Oracle-Client.csproj
COPY . .
WORKDIR /src/
RUN dotnet build Oracle-Client.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Oracle-Client.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Oracle-Client.dll"]
