const mongoose = require('mongoose');
mongoose.Promise = global.Promise;

var players = new mongoose.Schema({
    id: {
        type: Number,
        required: true, unique: true
    },
    username: {
        type: String,
        required: true, unique: true
    },
    password: {
        type: String,
        required: true
    },
    money: {
        type: Number,
        default: 0
    },
    status: {
        type: String,
        default: 0
    },
    email: {
        type: String,
        required: true, unique: true
        
    },
    nickname: {
        type: String,
        required: true, unique: true
    },
    avartar: {
        type: String
    }
});


module.exports = mongoose.model('players', players, "players");