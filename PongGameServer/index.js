const express = require('express');
const app = express();
var server = require('http').createServer();
var cors = require("cors");
const corsOptions = {
  origin: "*",
  optionsSuccessStatus: 200
};
const io = require("socket.io")(server, {
  cors: {
    origin: "*",
    methods: ["PUT", "GET", "POST", "DELETE", "OPTIONS"],
    credentials: false
  }
});
app.use(cors(corsOptions));

// app.get('/', (req, res) => {
//   res.sendFile(__dirname + '/index.html');
// });

// io.on('connection', (socket) => {
//   console.log('a user connected');
//   io.emit('chat message');
//   socket.on('chat message', (msg) => {
//     io.emit('chat message', msg);
//   });
//   socket.on('beep', function () {
//     socket.emit('boop');
//   });
//   socket.on('disconnect', () => {
//     console.log('user disconnected');
//   });
// });

server.listen(3000, () => {
  console.log('listening on *:3000');
});

const Server = require("./src/network/server")
console.log('Server has started');

let gameServer = new Server();

setInterval(() => {
  gameServer.onUpdate();
}, 100, 0);

io.on('connection', function(socket) {
    let connection = gameServer.onConnected(socket);
    connection.createEvents();
    connection.socket.emit('register', {'id': connection.player.id});
});

function interval(func, wait, times) {
    var interv = function(w, t){
        return function(){
            if(typeof t === "undefined" || t-- > 0){
                setTimeout(interv, w);
                try{
                    func.call(null);
                }
                catch(e){
                    t = 0;
                    throw e.toString();
                }
            }
        };
    }(wait, times);

    setTimeout(interv, wait);
}
