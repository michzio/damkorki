# damkorki\n
\n
DamkorkiWebApi\n
\n
$ cd ./Developer/damkorki/damkorki_web_api/damkorki_web_api \n
$ sudo docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=PaSSword12!' -p 1433:1433 -d microsoft/mssql-server-linux \n
$ docker ps \n
$ \n
$ dotnet ef database update \n
$ dotnet run \n
\n
DamkorkiApp \n
\n
$ cd ./Developer/damkorki/damkorki_app \n
$ dotnet run \n