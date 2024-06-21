# StockMarket
Technical task: Includes two main projects: Rest API and WebSocket projects

All apps are written in .net8, to run it you need to have visual studio or other code redactor that can run .net apps.

Instructions for running Rest API.

Put StockMarket.Api as a startup project and run it.
To use a public API like Alpha Vantage or CEX.io to fetch live price, you need to get api key and make sure that API key is valed.
The API key is stored in the appsettings.json file of the Stock Market.Api application.

Instructions for running WebSocket.

There are two sub projects StockMarketWebSocket and SimpleWS-Client to demonstrate comunication beetween client and server apps 

In visual studio go to configure startup projects... and select Multiple startup projects ans select StockMarketWebSocket and SimpleWS-Client.
