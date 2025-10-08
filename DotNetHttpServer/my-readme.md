# DATE: 27-August-2025

# Asp.Net
# Create the app
app="DotNetHttpServer"
dotnet new webapp -n $app --framework net9.0
cd $app

# dotnet commands
dotnet clean
dotnet restore
dotnet build

## Creates folder bin/publish
dotnet publish

dotnet run

## Test
http://localhost:5087/health
http://localhost:5087/environment
http://localhost:5087/variables
http://localhost:5087/request

http://localhost:5087/postjson