const shortID = require('shortid');
const Vector2 = require('./utils/vector2');

module.exports = class Ball {
    constructor() {
        this.id = shortID.generate();
        this.name = "Ball"
        this.speed = 0.5;
        this.position = new Vector2();
        this.direction = new Vector2(1,2);
       
    }

    onUpdate() {     
        this.position.x += this.direction.x * this.speed;
        this.position.y += this.direction.y * this.speed;
        console.log("x " +this.position.x)
        console.log("y " +this.position.y)
    }
}