cd ..\src\Library.Web.Mvc
dotnet publish -f netcoreapp2.0 -c Release
xcopy bin\Release\netcoreapp2.0\publish ..\..\dist /sy
