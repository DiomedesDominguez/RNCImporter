FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.1-disco AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.101-disco AS build
WORKDIR /src
COPY ["DNMOFT.RNC/DNMOFT.RNC.csproj", "DNMOFT.RNC/"]
RUN dotnet restore "DNMOFT.RNC/DNMOFT.RNC.csproj"
COPY . .
WORKDIR "/src/DNMOFT.RNC"
RUN dotnet build "DNMOFT.RNC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DNMOFT.RNC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DNMOFT.RNC.dll"]