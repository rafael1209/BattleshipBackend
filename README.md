# BattleshipBackend

## Development

`appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Google": {
    "ClientId": "GOOGLE_CLIENT_ID",
    "ClientSecret": "GOOGLE_SECRET_ID",
    "RedirectUri": "GOOGLE_REDIRECT_URI"
  },
  "Discord": {
    "ClientId": "DISCORD_CLIENT_ID",
    "ClientSecret": "DISCORD_CLIENT_SECRET",
    "RedirectUri": "DISCORD_REDIRECT_URI"
  },
  "MongoDb": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "battleship-bd"
  },
  "Jwt": {
    "SecretKey": "JWT_SECRET_KEY",
    "Issuer": "JWT_ISSUER"
  }
}
```

## Docker

`docker-compose.yml`

```yml
version: '3.8'

services:
  battleship-back:
    image: ghcr.io/rafael1209/battleshipbackend:master
    container_name: battleship-back
    restart: always
    expose:
      - "8080"
    environment:
      Google:ClientId: "GOOGLE_CLIENT_ID"
      Google:ClientSecret: "GOOGLE_SECRET_ID"
      Google:RedirectUri: "GOOGLE_REDIRECT_URI"
      Discord:ClientId: "DISCORD_CLIENT_ID"
      Discord:ClientSecret: "DISCORD_CLIENT_SECRET"
      Discord:RedirectUri: "DISCORD_REDIRECT_URI"
      MongoDb:ConnectionString: "mongodb://mongodb:27017/"
      MongoDb:DatabaseName: "battleship-bd"
      Jwt:SecretKey: "JWT_SECRET_KEY"
      Jwt:Issuer: "JWT_ISSUER"
    networks:
      - app-network
      - mongo-network
    
  mongodb:
    image: mongo:latest
    container_name: mongodb
    restart: always
    ports:
      - "27017:27017"
    volumes:
      - ./mongo-data:/data/db
    networks:
      - mongo-network

  nginx:
    image: nginx:alpine
    container_name: nginx
    restart: always
    ports:
      - "80:80"
      - "443:443"
    volumes:
      - ./nginx-conf:/etc/nginx/conf.d
    networks:
      - app-network
      
networks:
  app-network:
    driver: bridge
  mongo-network:
    driver: bridge
```
