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
COPY ["src/Pandora.ShortLink/Pandora.ShortLink.csproj", "src/Pandora.ShortLink/"]
COPY ["src/Pandora.ShortLink.Core/Pandora.ShortLink.Core.csproj", "src/Pandora.ShortLink.Core/"]
RUN dotnet restore "src/Pandora.ShortLink/Pandora.ShortLink.csproj"
COPY . .
WORKDIR "/src/src/Pandora.ShortLink"
RUN dotnet build "Pandora.ShortLink.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pandora.ShortLink.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pandora.ShortLink.dll"]