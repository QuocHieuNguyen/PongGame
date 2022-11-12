const shortID = require('shortid');
const Vector2 = require('./utils/vector2');
const Queue = require('./utils/queue')
const  {SERVER_TICK_RATE} = require('./utils/constant')
module.exports = class Ball {
    constructor(connections) {
        this.id = shortID.generate();
        this.name = "Ball"
        this.speed = 0.1;
        this.position = new Vector2();
        this.direction = new Vector2(-1,2);
        this.connections = connections

        // client prediction
        this.timer = 0
        this.minTimeBetweenTicks = 1.0/SERVER_TICK_RATE
        this.stateBuffer = []
        for(let i = 0; i < BUFFER_SIZE; i++) {
            this.stateBuffer.push(null);
        }
        this.inputQueue = new Queue()
    }

    onUpdate() {   
        this.timer += DELTA_TIME
        while(this.timer >= this.minTimeBetweenTicks){
            this.timer -= this.minTimeBetweenTicks
            this.HandleTick();
        }
        this.HandleMovement();
    }
    HandleTick(){
        let bufferIndex = -1
        while(this.inputQueue.length > 0){
            inputPayload = this.inputQueue.dequeue()
            bufferIndex = inputPayload.tick % BUFFER_SIZE

            statePayload = this.ProcessMovement(inputPayload)
            this.stateBuffer[bufferIndex] = statePayload

        }
        if (bufferIndex != -1){
            this.connections.forEach(connection => {
                connection.socket.emit('updateBallBufferState', bufferIndex);
            });
        }
    }
    EnqueueClientInput(inputPayload){
        this.inputQueue.enqueue(inputPayload)
    }
    ProcessMovement(input){
        this.position = input.inputVector * this.minTimeBetweenTicks

        return {
            tick : input.tick,
            position : this.position
        }
    }
    HandleMovement() {
        //this.position.x = this.lerp(this.position.x, this.position.x+this.direction.x * this.speed, 1);  
        //this.position.y = this.lerp(this.position.y, this.position.y+this.direction.y * this.speed, 1);  
        this.position.x += this.direction.x * this.speed;
        this.position.y += this.direction.y * this.speed;

        let returnData = {
            id: this.id,
            position: {
                x: this.position.x,
                y: this.position.y
            }
        };

        this.connections.forEach(connection => {
            connection.socket.emit('updateBallPosition', returnData);
        });
    }

    reflectDirection(isHorizontally){
        if (isHorizontally){
            this.direction = new Vector2(-this.direction.x, this.direction.y)
        }else{
            this.direction = new Vector2(this.direction.x, -this.direction.y)
        }
    }
    lerp(a, b, amount){
        return (1 - amount) * a + amount * b;
    }
}