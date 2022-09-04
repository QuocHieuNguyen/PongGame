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

app.get('/', (req, res) => {
  res.sendFile(__dirname + '/index.html');
});

clientHandlers = []

io.on('connection', (socket) => {
  let currentPlayer = {};

  console.log('a user connected');
  io.emit('chat message');
  socket.on('chat message', (msg) => {
    io.emit('chat message', msg);
  });
  socket.on('beep', function () {
    socket.emit('boop');
  });

  socket.on('play', (data)=>{
    console.log(data)
    currentPlayer.name = data.name;

    let dataLength = {
      length: clientHandlers.length
    }
    socket.emit('play', dataLength)
    socket.broadcast.emit('other player connected', currentPlayer);
    clientHandlers.push(currentPlayer)
  })
  socket.on('player move', function(data) {
		
    //let moveData = JSON.stringify(data);
    console.log('move: '+ data["position"]);
    console.log('move: '+ data);
    console.log('move: '+ data.position);
    socket.emit('player move', data)
		socket.broadcast.emit('other player move', data);
	});
  socket.on('disconnect', () => {
    console.log('user disconnected');
		for(let i=0; i<clientHandlers.length; i++) {
			if(clientHandlers[i].name === currentPlayer.name) {
				clientHandlers.splice(i,1);
			}
		}  });
});

server.listen(3000, () => {
  console.log('listening on *:3000');
});
