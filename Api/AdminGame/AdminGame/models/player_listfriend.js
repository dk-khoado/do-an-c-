const mongoose = require('mongoose');
mongoose.Promise = global.Promise;

var friends = new mongoose.Schema({
    
});


module.exports = mongoose.model('player_listfriend', friends);