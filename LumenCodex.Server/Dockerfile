FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY *.sln ./
COPY Services/*.csproj ./Services/
COPY LumenCodex.Server/*.csproj ./LumenCodex.Server/

RUN dotnet restore LumenCodex.Server/LumenCodex.Server.csproj

COPY . ./
RUN dotnet publish LumenCodex.Server/LumenCodex.Server.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 8080

ENTRYPOINT [ "dotnet", "LumenCodex.Server.dll" ]
