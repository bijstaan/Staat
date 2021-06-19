FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY bin/Release/netcoreapp5.0/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "aspnetapp.dll"]