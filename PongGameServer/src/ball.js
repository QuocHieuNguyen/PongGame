const shortID = require('shortid');
const Vector2 = require('./utils/vector2');
const Constant = require('./utils/constant') 
module.exports = class Ball {
    constructor() {
        this.id = shortID.generate();
        this.name = "Ball"
        this.speed = 0.02;
        this.position = new Vector2();
        this.direction = new Vector2(-1,2);
       
    }

    onUpdate() {   
        //this.position.x = this.lerp(this.position.x, this.position.x+this.direction.x * this.speed, 1);  
        //this.position.y = this.lerp(this.position.y, this.position.y+this.direction.y * this.speed, 1);  
         this.position.x += this.direction.x * this.speed;
         this.position.y += this.direction.y * this.speed;
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