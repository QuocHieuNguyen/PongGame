// import
const express = require('express');
const app = express();
const racket = require('./src/services/racket.service')
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
// cors
app.use(cors(corsOptions));

app.get('/', (req, res) => {
  res.sendFile(__dirname + '/index.html');
});

global._io = io; 
io.on('connection', (socket) => {
  console.log('a user connected');

  socket.on('chat message', racket.move);

  console.log(socket.id);
  socket.on('beep', function () {
    socket.emit('boop');
  });
  socket.on('disconnect', () => {
    console.log('user disconnected');
  });
});

server.listen(3000, () => {
  console.log('listening on *:3000');
});
