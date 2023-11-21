FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NetCore.BackendServer/NetCore.BackendServer.csproj", "NetCore.BackendServer/"]
COPY ["NetCore.ViewModels/NetCore.ViewModels.csproj", "NetCore.ViewModels/"]
RUN dotnet restore "NetCore.BackendServer/NetCore.BackendServer.csproj"
COPY . .
WORKDIR "/src/NetCore.BackendServer"
RUN dotnet build "NetCore.BackendServer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NetCore.BackendServer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# Thêm các dòng sau vào cuối file Dockerfile
ENV ASPNETCORE_URLS=https://+:443;http://+:80
# Đường dẫn tới certificate và private key
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/certificate.pfx
# Mật khẩu của certificate (nếu có)
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=Phuduc@30072001

ENTRYPOINT ["dotnet", "NetCore.BackendServer.dll"]