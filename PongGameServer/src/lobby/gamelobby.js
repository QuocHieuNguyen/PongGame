const Connection = require("../network/connection")
const lobbyBase = require("./lobbybase")

const lobbySetting = require("./lobby.setting")

module.exports = class GameLobbby extends lobbyBase {
    constructor(id, settings = lobbySetting) {
        super(id);
        this.settings = settings;
        this.hostPlayer = null;
        this.isGameStarted = false;
        //this.bullets = [];
    }

    onUpdate() {
        let lobby = this;

        //lobby.updateBullets();
        //lobby.updateDeadPlayers();
    }

    canEnterLobby(connection = Connection) {
        let lobby = this;
        let maxPlayerCount = lobby.settings.maxPlayers;
        let currentPlayerCount = lobby.connections.length;

        if(currentPlayerCount + 1 > maxPlayerCount) {
            return false;
        }

        return true;
    }

    onEnterLobby(connection = Connection) {
        let lobby = this;

        super.onEnterLobby(connection);

        lobby.addPlayer(connection);
        lobby.connections.forEach(_connection => {
            
            _connection.socket.emit('New Player Enter Lobby', connection.player.id);
        });
        if (lobby.connections.length == 1){
            this.hostPlayer = connection;
        }
        if (lobby.connections.length == 2){
            this.guestPlayer = connection;
        }
        //Handle spawning any server spawned objects here
        //Example: loot, perhaps flying bullets etc
    }

    onLeaveLobby(connection = Connection) {
        let lobby = this;

        super.onLeaveLobby(connection);

        lobby.removePlayer(connection);

        //Handle unspawning any server spawned objects here
        //Example: loot, perhaps flying bullets etc
    }
    onDisplayLobbyData(connection = Connection){
        let lobby = this;
        //lobby.displayLobbyData();
        //console.log("call this")
        //connection.socket.emit('get lobby data', lobby.connections[0].player);
    }
    onDisplayLobbyPlayerData(connection = Connection){
        let playerIDs = []
        let lobby = this;
        let connections = lobby.connections;
        console.log("display player data")
        connections.forEach(_connection => {
            playerIDs.push(_connection.player.id)
            //connection.socket.emit('get lobby data', lobby);
        });
        let payload = {
            "host_name" : connections[0].player.username,
            "role": "player"
        }
        if (connections.length > 1){
            payload.opponent_name = connections[1].player.username
        }
        if (connection.player.id == lobby.hostPlayer.player.id){
            payload.role = "host"
        }

        connection.socket.emit('DisplayPlayerData', payload)
    }
    displayLobbyData(){
        let lobby = this;
        let connections = lobby.connections;

        connections.forEach(connection => {
            //connection.socket.emit('get lobby data', lobby);
        });
    }
    updateBullets() {
        let lobby = this;
        let bullets = lobby.bullets;
        let connections = lobby.connections;

        bullets.forEach(bullet => {
            let isDestroyed = bullet.onUpdate();

            if(isDestroyed) {
                lobby.despawnBullet(bullet);
            } else {
                var returnData = {
                    id: bullet.id,
                    position: {
                        x: bullet.position.x,
                        y: bullet.position.y
                    }
                }

                connections.forEach(connection => {
                    connection.socket.emit('updatePosition', returnData);
                });
            }
        });
    }
    StartGame(connection = Connection){
        let lobby = this;
        if (lobby.hostPlayer.player.id == connection.player.id){
            console.log("Permission granted to host: Start Game ")
            lobby.isGameStarted = true;
            lobby.connections.forEach(_connection => {
                _connection.socket.emit('GameIsStarted');
            });
        }else{
            console.log("no start game permission granted to non-host")
        }
    }

    updateDeadPlayers() {
        let lobby = this;
        let connections = lobby.connections;

        connections.forEach(connection => {
            let player = connection.player;

            if(player.isDead) {
                let isRespawn = player.respawnCounter();
                if(isRespawn) {
                    let socket = connection.socket;
                    let returnData = {
                        id: player.id,
                        position: {
                            x: player.position.x,
                            y: player.position.y
                        }
                    }

                    socket.emit('playerRespawn', returnData);
                    socket.broadcast.to(lobby.id).emit('playerRespawn', returnData);
                }
            }
        });
    }

    onFireBullet(connection = Connection, data) {
        let lobby = this;

        let bullet = new Bullet();
        bullet.name = 'Bullet';
        bullet.activator = data.activator;
        bullet.position.x = data.position.x;
        bullet.position.y = data.position.y;
        bullet.direction.x = data.direction.x;
        bullet.direction.y = data.direction.y;

        lobby.bullets.push(bullet);

        var returnData = {
            name: bullet.name,
            id: bullet.id,
            activator: bullet.activator,
            position: {
                x: bullet.position.x,
                y: bullet.position.y
            },
            direction: {
                x: bullet.direction.x,
                y: bullet.direction.y
            }
        }

        connection.socket.emit('serverSpawn', returnData);
        connection.socket.broadcast.to(lobby.id).emit('serverSpawn', returnData); //Only broadcast to those in the same lobby as us
    }

    onCollisionDestroy(connection = Connection, data) {
        let lobby = this;

        let returnBullets = lobby.bullets.filter(bullet => {
            return bullet.id == data.id
        });

        returnBullets.forEach(bullet => {
            let playerHit = false;

            lobby.connections.forEach(c => {
                let player = c.player;

                if(bullet.activator != player.id) {
                    let distance = bullet.position.Distance(player.position);

                    if(distance < 0.65) {
                        let isDead = player.dealDamage(50);
                        if(isDead) {
                            console.log('Player with id: ' + player.id + ' has died');
                            let returnData = {
                                id: player.id
                            }
                            c.socket.emit('playerDied', returnData);
                            c.socket.broadcast.to(lobby.id).emit('playerDied', returnData);
                        } else {
                            console.log('Player with id: ' + player.id + ' has (' + player.health + ') health left');
                        }
                        lobby.despawnBullet(bullet);
                    }
                }
            });

            if(!playerHit) {
                bullet.isDestroyed = true;
            }
        });        
    }

    despawnBullet(bullet = Bullet) {
        let lobby = this;
        let bullets = lobby.bullets;
        let connections = lobby.connections;

        console.log('Destroying bullet (' + bullet.id + ')');
        var index = bullets.indexOf(bullet);
        if(index > -1) {
            bullets.splice(index, 1);

            var returnData = {
                id: bullet.id
            }

            //Send remove bullet command to players
            connections.forEach(connection => {
                connection.socket.emit('serverUnspawn', returnData);
            });
        }
    }

    addPlayer(connection = Connection) {
        let lobby = this;
        let connections = lobby.connections;
        let socket = connection.socket;

        var returnData = {
            id: connection.player.id
        }

        socket.emit('spawn', returnData); //tell myself I have spawned
        socket.broadcast.to(lobby.id).emit('spawn', returnData); // Tell others

        //Tell myself about everyone else already in the lobby
        connections.forEach(c => {
            if(c.player.id != connection.player.id) {
                socket.emit('spawn', {
                    id: c.player.id
                });
            }
        });
    }

    removePlayer(connection = Connection) {
        let lobby = this;

        connection.socket.broadcast.to(lobby.id).emit('disconnected', {
            id: connection.player.id
        });
    }
}