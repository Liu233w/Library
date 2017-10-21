cd ../src/Library.Migrator
dotnet publish -f netcoreapp2.0 -c Release
cp -r bin/Release/netcoreapp2.0/publish/* ../../migrator
