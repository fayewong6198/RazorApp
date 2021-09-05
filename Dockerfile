FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY *.sln .
COPY razorapp/RazorApp.csproj ./razorapp/
RUN dotnet restore


COPY razorapp/. ./razorapp/
WORKDIR /src/razorapp
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
EXPOSE 80
ENTRYPOINT ["dotnet", "RazorApp.dll"]