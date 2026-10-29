dotnet restore

dotnet build

dotnet run --project .\API --launch-profile Localhost

start http://localhost:5080/swagger

check db mở chưa
   docker version
   docker start oracle-xe
   check chuỗi kết nối dotnet user-secrets list --project .\API

dotnet build
dotnet run --project .\API --launch-profile Localhost