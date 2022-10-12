const shortID = require('shortid');
const Vector2 = require('./utils/vector2');

module.exports = class Ball {
    constructor() {
        this.id = shortID.generate();
        this.name = "Ball"
        this.speed = 1;
        this.position = new Vector2();
        this.direction = new Vector2(1,0);
       
    }

    onUpdate() {   
        this.position.x = this.lerp(this.direction.x, this.direction.x * this.speed, 0.4);  
        this.position.y = this.lerp(this.direction.y, this.direction.y * this.speed, 0.4);  
        //this.position.x += this.direction.x * this.speed;
        //this.position.y += this.direction.y * this.speed;

    }
    lerp(a, b, amount){
        return (1 - amount) * a + amount * b;
    }
}