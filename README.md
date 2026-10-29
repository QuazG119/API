dotnet restore

dotnet build
dotnet user-secrets set "ConnectionStrings:PLSQLServerPartnerWrite" "User Id=system;Password=MyPass123;Data Source=localhost:1521/FREEPDB1" --project .\API
Get-Content scripts\oracle-init.sql | docker exec -i oracle-xe sqlplus -s system/MyPass123@//localhost:1521/FREEPDB1

dotnet run --project .\API --launch-profile Localhost

start http://localhost:5080/swagger


   docker start oracle-xe