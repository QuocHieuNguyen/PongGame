## About The Project

_This project is still under development and there are issues the author should work on (e.g lag issue, UI improvement,....), it will be updated overtime_

The project is a Pong Online Game, handling basic functionalities such as:
- [x] Match Selection
- [x] Multiple Game Rooms in one server

Some features might be added in the future:
- [ ] Spectator mode
- [ ] Diverse Game Mode

The project is inspired by two other projects:
- Client: 
[Kalmalyzer/Pong](https://github.com/Kalmalyzer/Pong)
- Server: 
[Multiplayer_Game_With_Unity_And_NodeJS](https://github.com/oohicksyoo/Youtube-Multiplayer_Game_With_Unity_And_NodeJS)

Overall architecture of the client:

![Overall architecture image](/docs/Pong.drawio.png "Overall architecture")

For each entity, the MonoBehavior class acts as a facade, containing separated classes for separated logic.

In this case, the network class has the responsibilty of communicating to the server, including notifying the logic class about server-sent event, or emit the event to the server.