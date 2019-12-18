const mongoose = require('mongoose');
mongoose.Promise = global.Promise;
var shopGame = new mongoose.Schema({
    id_item: {
        type: Number,
        required: true,
        unique: true
    },
    name_item: {
        type: String,
        required: true,
        unique: true
    },
    limit_day: {
        type: Date
    },
    cost: {
        type: Number,
        required: true
    },
    image: {
        type: String
    },
    isEnable: {
        type: Boolean
    },
    type_item: {
        type: String,
        required: true
    },
    duration_day: {
        type: Date
    },
    descript: {
        type: String
    }
});

module.exports = mongoose.model('shop_game', shopGame);