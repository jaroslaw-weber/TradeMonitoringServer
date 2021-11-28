# Trade Monitoring - Server

Simulate executing and monitoring trades of securities (server-side). When connected by websocket to this server, server will push update every second.

All data is fake but is made to look natural (random trades executed at random times, updating the position list)

### How to run

```
cd TradeMonitoringServer
dotnet run
```

### How to run tests
```
dotnet test
```

### TODO
Things I did not have time to implement but could use improvement:
- stop pushing updates to client when client disconnects
- write more tests
- specify port in config
- add gif to readme showing functionality