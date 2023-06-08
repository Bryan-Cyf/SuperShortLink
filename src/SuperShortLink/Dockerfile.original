#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV TZ Asia/Shanghai
RUN set -eux; \
    sed -i 's/dl-cdn.alpinelinux.org/mirrors.aliyun.com/g' /etc/apk/repositories; \
    apk add tzdata; \
    cp /usr/share/zoneinfo/${TZ} /etc/localtime; \
    echo ${TZ} > /etc/timezone; \
    rm -rf /var/cache/apk/*; \
    rm -rf /root/.cache; \
    rm -rf /tmp/*

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/SuperShortLink/SuperShortLink.csproj", "src/SuperShortLink/"]
COPY ["src/SuperShortLink.Core/SuperShortLink.Core.csproj", "src/SuperShortLink.Core/"]
RUN dotnet restore "src/SuperShortLink/SuperShortLink.csproj"
COPY . .
WORKDIR "/src/src/SuperShortLink"
RUN dotnet build "SuperShortLink.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SuperShortLink.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SuperShortLink.dll"]