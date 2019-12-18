const mongoose = require('mongoose');
mongoose.Promise = global.Promise;
var addFriend = new mongoose.Schema({
    player_id: {
        type: Number,
        required: true
    },

    friend_id: {
        type: Number,
        required: true
    },
    isban: {
        type: String
    }
});

module.exports = mongoose.model('AddFriendModel', addFriend);