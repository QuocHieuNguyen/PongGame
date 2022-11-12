

module.exports = class Connection {
    constructor() {
        this.socket;
        this.player;
        this.server;
        this.lobby;
    }

    //Handles all our io events and where we should route them too to be handled
    createEvents() {
        let connection = this;
        let socket = connection.socket;
        let server = connection.server;
        let player = connection.player;

        socket.on('disconnect', function() {
            server.onDisconnected(connection);
        });

        socket.on('joinGame', function() {
            server.onAttemptToJoinGame(connection);
        });

        socket.on('fetchAllLobbyData', ()=>{
            server.onDisplayLobbyData(connection)
        });
        socket.on('createNewLobby', ()=>{
            server.OnCreateNewLobby(connection)
        })
        socket.on('fetchCurrentLobby', (id)=>{
            server.OnFetchCurrentLobby(connection, id)
        })
        socket.on('joinSpecificLobby', (id)=>{
            server.JoinSpecificLobby(connection, id)
        })
        socket.on('hostGame', ()=>{
            server.HostGame(connection)
        })
        socket.on('leftRoom', ()=>{
            server.LeftGame(connection)
        })
        socket.on('fetchPlayerDataInLobby', ()=>{
            server.DisplayLobbyPlayerData(connection)
        })
        socket.on('setName', (name)=>{
            server.SetPlayerName(connection, name);
        })
        socket.on('getName', ()=>{
            server.GetName(connection)
        })
        socket.on('startGame', ()=>{
            server.StartGame(connection)
        })
        socket.on('updatePosition', (pos)=>{
            server.UpdatePosition(connection, pos)
        })
        socket.on('reflectFromWall', ()=>{
            server.ReflectFromWall(connection)
        })
        socket.on('reflectFromPaddle', ()=>{
            server.ReflectFromPaddle(connection)
        })
        socket.on('playerIsLose', ()=>{
            server.PlayerIsLose(connection)
        })
        socket.on('updateBallBufferInput', (inputPayload)=> {
            server.HandleBallBufferInput(connection, inputPayload)
            console.log("connection.js: " + inputPayload)
        })
    }
}