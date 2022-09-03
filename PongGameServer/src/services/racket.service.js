const constants = require('../constants/constants')
module.exports = {
     move(msg) {
        // _io.emit(constants.MOVE, {
        //     x:1,
        //     y:1
        // });
        console.log(msg)
        _io.emit(constants.MOVE, 1);
    }
}