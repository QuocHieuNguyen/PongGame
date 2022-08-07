const express = require('express');
const app = express();
const server = require('http').Server(app);
io = require('socket.io')(server);

app.listen(3000, () => console.log('started and listening.'));

app.get('/', (req, res) => {
    res.send('Hello Unity Developers!');
})
io.on('connection', function(socket){
    socket.on('beep', function(){
        socket.emit('boop');
    })
});