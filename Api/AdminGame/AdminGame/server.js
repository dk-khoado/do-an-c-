'use strict';
var express = require('express');
var app = express();
var http = require('http').createServer(app);
var port = process.env.PORT || 1337;

var MongoClient = require('mongoose');
var url = "mongodb://localhost:27017/GameBai";
MongoClient.connect(url, { useUnifiedTopology: true, useNewUrlParser: true }, function (err, db) {
    if (err) throw err;
    console.log("Connection ok");
});



app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(express.static(__dirname + '/public/'));

app.engine('html', require('ejs').renderFile);

var admin = require('./controller/Admin');
app.use('/', admin);

var player = require('./controller/Api');
app.use('/Api/Players', player);



http.listen(port, function (err) {
    if (err) { console.log(err); }
    else {
        console.log('Listening on ' + port);
    }
});

