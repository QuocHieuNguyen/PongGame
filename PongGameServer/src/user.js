const shortID = require('shortid');

module.exports = class User {
    constructor() {
        this.username = 'Default_Player';
        this.id = shortID.generate();
        this.lobby = 0;
    }

    displayerPlayerInformation() {
        let player = this;
        return '(' + player.username + ':' + player.id + ')';
    }

}