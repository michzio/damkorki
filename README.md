# damkorki<br />
<br />
DamkorkiWebApi<br />
<br />
$ cd ./Developer/damkorki/damkorki_web_api/damkorki_web_api <br />
$ sudo docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=PaSSword12!' <br />
-p 1433:1433 -d microsoft/mssql-server-linux <br />
$ docker ps <br />
$ <br />
$ dotnet ef database update <br />
$ dotnet run <br />
<br />
DamkorkiApp <br />
<br />
$ cd ./Developer/damkorki/damkorki_app <br />
$ dotnet run <br />