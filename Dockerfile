
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS staat_build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Staat/*.csproj ./Staat/
RUN dotnet restore -r alpine-x64

# copy everything else and build app
COPY Staat/. ./Staat/
COPY docker/appsettings.json ./Staat/appsettings.json

WORKDIR /source/Staat
RUN dotnet publish -c release -o /app -r alpine-x64 --no-restore -p:PublishSingleFile=true --self-contained true
# /p:PublishTrimmed=false /p:PublishReadyToRun=false

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine-amd64
WORKDIR /app
COPY --from=staat_build /app ./

# See: https://github.com/dotnet/announcements/issues/20
# Uncomment to enable globalization APIs (or delete)
ENV \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8
RUN apk add --no-cache icu-libs

# Uncomment to hang the container to allow for debugging
# ENTRYPOINT ["tail", "-f", "/dev/null"]

ENTRYPOINT ["./Staat"]