const mongoose = require('mongoose');

mongoose.Promise = global.Promise;

var ApiKey = new mongoose.Schema({
    id: {
        type: Number,
        required: true,
        unique: true
    },
    username: {
        type: String,
        required: true,
        unique: true
    },
    player_key: {
        type: String,
        required: true,
        unique: true
    }
});


module.exports = mongoose.model('ApiKey', ApiKey, "ApiKey");