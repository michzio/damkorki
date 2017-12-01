# damkorki

DamkorkiWebApi 

$ cd ./Developer/damkorki/damkorki_web_api/damkorki_web_api
$ sudo docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=PaSSword12!' -p 1433:1433 -d microsoft/mssql-server-linux
$ docker ps 
$
$ dotnet ef database update 
$ dotnet run

DamkorkiApp

$ cd ./Developer/damkorki/damkorki_app
$ dotnet run

