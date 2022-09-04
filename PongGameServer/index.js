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

io.on('connection', (socket) => {
  console.log('a user connected');
  io.emit('chat message');
  socket.on('chat message', (msg) => {
    io.emit('chat message', msg);
  });
  socket.on('beep', function () {
    socket.emit('boop');
  });
  socket.on('player move', function(data) {
		
    //let moveData = JSON.stringify(data);
    console.log('move: '+ data["position"]);
    console.log('move: '+ data);
    console.log('move: '+ data.position);
    socket.emit('player move', data)
		//socket.broadcast.emit('player move', currentPlayer);
	});
  socket.on('disconnect', () => {
    console.log('user disconnected');
  });
});

server.listen(3000, () => {
  console.log('listening on *:3000');
});
