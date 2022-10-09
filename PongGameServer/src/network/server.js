const Connection = require('./connection')
const Player = require('../user')

//Lobbies
const LobbyBase = require('../lobby/lobbybase')
const GameLobby = require('../lobby/gamelobby')
const GameLobbySettings = require('../lobby/lobby.setting')


module.exports = class Server {
    constructor() {
        this.connections = [];
        this.lobbys = [];

        this.lobbys[0] = new LobbyBase(0);
    }

    //Interval update every 100 miliseconds
    onUpdate() {
        let server = this;

        //Update each lobby
        for(let id in server.lobbys) {
            server.lobbys[id].onUpdate();
        }
    }

    //Handle a new connection to the server
    onConnected(socket) {
        let server = this;
        let connection = new Connection();
        connection.socket = socket;
        connection.player = new Player();
        connection.server = server;

        let player = connection.player;
        let lobbys = server.lobbys;

        console.log('Added new player to the server (' + player.id + ')');
        server.connections[player.id] = connection;

        socket.join(player.lobby);
        connection.lobby = lobbys[player.lobby];
        connection.lobby.onEnterLobby(connection);

        return connection;
    }

    onDisconnected(connection = Connection) {
        let server = this;
        let id = connection.player.id;

        delete server.connections[id];
        console.log('Player ' + connection.player.displayerPlayerInformation() + ' has disconnected');

        //Tell Other players currently in the lobby that we have disconnected from the game
        connection.socket.broadcast.to(connection.player.lobby).emit('disconnected', {
            id: id
        });

        //Preform lobby clean up
        server.lobbys[connection.player.lobby].onLeaveLobby(connection);
    }

    onAttemptToJoinGame(connection = Connection) {
        //Look through lobbies for a gamelobby
        //check if joinable
        //if not make a new game
        let server = this;
        let lobbyFound = false;

        let gameLobbies = server.lobbys.filter(item => {
            return item instanceof GameLobby;
        });
        console.log('Found (' + gameLobbies.length + ') lobbies on the server');

        gameLobbies.forEach(lobby => {
            if(!lobbyFound) {
                let canJoin = lobby.canEnterLobby(connection);

                if(canJoin) {
                    lobbyFound = true;
                    server.onSwitchLobby(connection, lobby.id);
                }
            }
        });

        //All game lobbies full or we have never created one
        if(!lobbyFound) {
            console.log('Making a new game lobby');
            let gamelobby = new GameLobby(gameLobbies.length + 1, new GameLobbySettings('FFA', 2));
            server.lobbys.push(gamelobby);
            server.onSwitchLobby(connection, gamelobby.id);
        }
    }
    onDisplayLobbyData(connection = Connection){
        let server = this;
        
        let lobbyIdArray = []
        for (let i = 0; i < server.lobbys.length; i++) {
            const element = server.lobbys[i];
            if (element instanceof GameLobby){
                lobbyIdArray.push(element.id)
            }
           
            
        }
        console.log(lobbyIdArray)
        let lobbyDataPayload = {
            "lobbyArray": lobbyIdArray
        }
        connection.socket.emit('onDisplayLobbyData', lobbyDataPayload)

    }
    OnCreateNewLobby(connection = Connection){
        let server = this
        let gamelobby = new GameLobby(server.lobbys.length + 1, new GameLobbySettings('FFA', 2));
        server.lobbys.push(gamelobby);
        connection.socket.emit('onCreateNewLobby', gamelobby)

    }
    OnFetchCurrentLobby(connection = Connection, id){
        let server = this
        let targetLobby = null;
        
        for (let i = 0; i < server.lobbys.length; i++) {
            const element = server.lobbys[i];
            if (element.id == id){
                targetLobby = element
            }else{
                console.log("not found " + element.id)
                
            }
        }
        if (targetLobby !== null){
            
            if (connection.player.lobby == targetLobby.id){
                connection.socket.emit('OnFetchCurrentLobby', targetLobby.id)
            }else{
                connection.socket.emit('OnFetchCurrentLobby', -2)
            }
        }else{
            connection.socket.emit('OnFetchCurrentLobby', -1)
        }
    }
    JoinSpecificLobby(connection = Connection, id){
        let server = this
        console.log("Player " + connection.player.username + " join room " + id)
        let gameLobbies = server.lobbys.filter(item => {
            return item.id == id;
        });
        console.log(gameLobbies.length)
        if (gameLobbies.length > 0){
            let payload = {
                "response_code": -1
            }
            connection.socket.emit('OnJoinSpecificLobby', payload)
            server.onSwitchLobby(connection, id)
        }else{
            let payload = {
                "response_code": -100
            }
            connection.socket.emit('OnJoinSpecificLobby', payload)
        }
        
    }
    onSwitchLobby(connection = Connection, lobbyID) {
        let server = this;
        let lobbys = server.lobbys;

        connection.socket.join(lobbyID); // Join the new lobby's socket channel
        connection.lobby = lobbys[lobbyID];//assign reference to the new lobby

        lobbys[connection.player.lobby].onLeaveLobby(connection);
        lobbys[lobbyID].onEnterLobby(connection);
    }
    HostGame(connection = Connection){
        let server = this
        let gameLobbies = server.lobbys.filter(item => {
            return item instanceof GameLobby;
        });
        let gamelobby = new GameLobby(gameLobbies.length + 1, new GameLobbySettings('FFA', 2));
        connection.socket.emit("hostSucceed")
        server.lobbys.push(gamelobby);
        server.onSwitchLobby(connection, gamelobby.id);
    }
    LeftGame(connection = Connection){
        let server = this;
        let lobbys = server.lobbys;

        lobbys[connection.player.lobby].onLeaveLobby(connection);
        lobbys[0].onEnterLobby(connection);
    }

    DisplayLobbyPlayerData(connection = Connection){
        let server = this;
        connection.lobby.onDisplayLobbyPlayerData(connection)
    }
    SetPlayerName(connection = Connection, name){
        connection.player.username = name;
        console.log("The new player name is " + connection.player.username);
    }
    GetName(connection = Connection){
        let payload = {
            "name" : connection.player.username
        }
        connection.socket.emit('GetName', payload)
    }
    StartGame(connection = Connection){
        connection.lobby.StartGame(connection)
    }
    UpdatePosition(connection = Connection, pos){
        let server = this
        let gameLobbies = server.lobbys.filter(item => {
            return item.id == connection.lobby.id;
        });
        if (gameLobbies.length > 0){
            gameLobbies[0].UpdatePosition(connection, pos)
        }
    }

}