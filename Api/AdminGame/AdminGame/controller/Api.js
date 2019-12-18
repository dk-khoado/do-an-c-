var express = require('express');
var router = express.Router();
var Player = require('../models/Players');
var apiKey = require('../models/ApiKey');
var MD5 = require('md5');
var nodemailer = require('nodemailer');
router.use(express.static(__dirname + '/public/'));


router.get('/GetAll', function (req, res) {
    Player.find({}, function (err, docs) {
        if (err) throw err;
        res.send(docs);
    })
});

router.post('/Register', function (req, res) {
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    for (var i = 0; i < 5; i++) {
        text += possible.charAt(Math.floor(Math.random() * possible.length));
    }

    let player = req.body;
    player.id = new Date().getTime();
    player.password = MD5(MD5(player.id) + player.password);

    Player.find({ username: player.username }).count(function (err, number) {
        if (err) {
            res.status(204).send(err);
        }
        else {
            if (number > 0) {
                res.status(200).send({ response: 200, message: "Username has exist !!!!", data: {}, result: 0 });
            }
            else {
                Player.find({ email: player.email }).count(function (err, number) {
                    if (err) {
                        res.status(204).send(err);
                    }
                    else {
                        if (number > 0) {
                            res.status(200).send({ response: 200, message: "Email has exist !!!!", data: {}, result: 0 });
                        }
                        else {
                            Player.create(player, function (err, result) {
                                if (err) {
                                    res.status(204).send(err);
                                } else {
                                    const option = {

                                        service: 'gmail',
                                        tls: {

                                            rejectUnauthorized: false
                                        },
                                        auth: {
                                            user: 'duy.haivl321@gmail.com',
                                            pass: 'fizz8206'
                                        },

                                    };
                                    var transporter = nodemailer.createTransport(option);


                                    console.log('Connect success!');
                                    var mail = {
                                        from: 'duy.haivl321@gmail.com',
                                        to: player.email,
                                        subject: 'Enter link to activate your account',
                                        html: "<a href='http://localhost:1337/Api/Players/ActiveAccount/" + player.id + "/" + text + "'>Enter to activate</a>"
                                    };

                                    transporter.sendMail(mail, function (error, info) {
                                        if (error) {
                                            
                                            res.status(204).send({ response: 204, message: "Send mail fail !!!!", data: [], result: 0 });
                                        } else {
                                            console.log('Email sent: ' + info.response);
                                            res.status(200).send({ response: 200, message: "Register success !!!!", data: result, result: 1 });
                                            apiKey.create({ id: player.id, username: player.username, player_key: text });
                                        }

                                    });
                                    
                                }
                            });
                        }
                    }

                })
            }
        }

    })


});

router.post('/Login', function (req, res) {
    let player = req.body;

    apiKey.find({ username: player.username }).count(function (err, number) {
        if (err) {
            res.status(204).send(err);
        }
        else {
            if (number > 0) {
                apiKey.findOne({ username: player.username }, function (err, result) {
                    if (err) {
                        res.status(204).send(err);
                    }
                    else {
                        player.password = MD5(MD5(result.id) + player.password);
                        Player.find({ username: player.username, password: player.password, status: 0 }).count(function (err, number) {
                            if (err) {
                                res.status(204).send(err);
                            } else {
                                if (number > 0) {
                                    res.status(200).send({ response: 200, message: "Login success !!!!", data: result, result: number });
                                } else {
                                    res.status(200).send({ response: 200, message: "Login fail !!!!", data: [], result: number });
                                }
                            }
                        });
                    }
                });
            } else {
                res.status(200).send({ response: 200, message: "Login fail !!!!", data: [], result: number });
            }

        }

    });
})

router.get('/ActiveAccount/:id/:key', function (req, res) {
    var player = req.body;
    var id = req.params.id;
    var key = req.params.key;
    Player.find({id: id}).count(function (err, number) {
        if (err) {
            res.status(204).send(err);
        }
        else {
            if (number > 0) {
                apiKey.find({ player_key: key }).count(function (err, number) {
                    if (err) {
                        res.status(204).send(err);
                    }
                    else {
                        if (number > 0) {
                            Player.update({ id: id }, { status: "1" }, function (err) {
                                if (err) {
                                    console.log(err);
                                }
                                else {
                                    res.render("../views/User/ActiveAccountSussess.html");
                                }
                            });
                        } else {
                            res.status(204).send({ response: 204, message: "Not found !!!", data: [], result: number });
                        }
                    }
                })
            } else {
                res.status(204).send({ response: 204, message: "Not found !!!", data: [], result: number });
            }
        }
    })
})

module.exports = router;