const mongoose = require('mongoose');
mongoose.Promise = global.Promise;
var history = new mongoose.Schema({
    id: {
        type: Number,
        required: true,
        unique: true
    },

    player_id: {
        type: Number,
        required: true
    },

    login_time: {
        type: Date
    },

    logout_time: {
        type: Date
    }
});

module.exports = mongoose.model('history_login', history);