FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /restaurant.server

COPY . ./

RUN dotnet restore

RUN dotnet publish -o out

FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /restaurant.server
COPY --from=build /restaurant.server/out .
EXPOSE 8080
ENTRYPOINT [ "dotnet", "restaurant.server.dll" ]