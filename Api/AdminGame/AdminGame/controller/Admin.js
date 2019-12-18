var express = require('express');
var router = express.Router();
var fs = require('fs');

router.use(express.static(__dirname + '/public/'));

router.get('/', function (req, res) {
    res.render("../views/layout.html", { content: 'LoginAdmin', err: '' });
});

router.post('/', function (req, res) {
    var admin = req.body;
    let check = false;
    fs.readFile('JSON.json', (err, data) => {
        if (err) throw err;
        let student = JSON.parse(data);
        for (let i = 0; i < student.length; i++) {
            if (student[i].name == admin.username && student[i].password == admin.password) {
                res.render("../views/Manager/AdminPage.html", { content: 'DashBoard' });
                check = true;
                break;
            }
        }
        if (check == false) {
            res.render("../views/layout.html", { content: 'LoginAdmin', err: 'Wrong username or password' });
        }
    });
});

router.get('/DashBoard', function (req, res) {
    res.render("../views/Manager/AdminPage.html", { content: 'DashBoard' });
});

router.get('/UpdateAdmin/:id', function (req, res) {
    let check = false;
    fs.readFile('JSON.json', (err, data) => {
        if (err) res.send({ message: "fail", result: 0 });
        let student = JSON.parse(data);
        for (var i = 0; i < student.length; i++) {
            if (student[i].id == req.params.id) {
                check = true;
                console.log(student[i].id);
                console.log(req.params.id);
                res.render("../views/layout.html", { content: 'Manager/UpdateAdmin', dataAdmin: JSON.stringify(student[i]) });
                break;
            }
        }
        if (check == false) {
            res.send({ message: "fail", result: 0 });
            
        }
    });
    
});

router.post('/UpdateAdmin/:id', function (req, res) {
    var admin = req.body;
    let check = false;
    
    fs.readFile('JSON.json', (err, data) => {
        if (err) res.send({ message: "fail", result: 0 });
        let student = JSON.parse(data);
        for (var i = 0; i < student.length; i++) {
            if (student[i].id == req.params.id) {
                check = true;
                student.splice(i, 1, { "id": student[i].id, "name": admin.adname, "password": student[i].password, "email": admin.ademail, "position": admin.adPosition });
                fs.writeFile('JSON.json', JSON.stringify(student), (err) => {
                    if (err) throw err;
                    console.log(student);
                });
                res.send({ message: "success", result: 1 });
                break;
            }
        }
        if (check == false) {
            res.send({ message: "fail", result: 0 });
        }
    });
});

router.post('/BanAdmin/:id', function (req, res) {
    let check = false;
    fs.readFile('JSON.json', (err, data) => {
        if (err) res.send({ message: "Fail", result:0 });
        let student = JSON.parse(data);
        for (var i = 0; i < student.length; i++) {
            if (student[i].id == req.params.id) {
                check = true;
                if (student[i].position == "1") {
                    res.send({ message: "Fail", result: 0 });
                } else {
                    student.splice(i, 1);
                    fs.writeFile('JSON.json', JSON.stringify(student), (err) => {
                        if (err) throw err;
                        console.log(student);
                    });
                    res.send({ message: "OK", result: 1 });
                }
                break;
            }
        }
        if (check == false) {
            res.send({ message: "Fail", result: 0 });
        }
    });
});


router.get('/Member', function (req, res) {
    fs.readFile('JSON.json', (err, data) => {
        if (err) throw err;
        let student = JSON.parse(data);
        
        
        res.render("../views/Manager/AdminPage.html", { content: 'Member', data: JSON.stringify(student) });
        
    });
    
});



router.get('/CreateAd', function (req, res) {
    res.render("../views/layout.html", { content: 'CreateAdmin', err: '' });
});

router.post('/CreateAd', function (req, res) {
    var admin = req.body;
    let check = true;
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    for (var i = 0; i < 5; i++) {
        text += possible.charAt(Math.floor(Math.random() * possible.length));
    }


    fs.readFile('JSON.json', (err, data) => {
        if (err) throw err;
        let student = JSON.parse(data);
        for (let i = 0; i < student.length; i++) {
            if (student[i].name == admin.adname || student[i].email == admin.ademail) {
                res.send({ message: "fail", result: 0 });
                check = false;
                break;
            }
        }
        if (check == true) {
            student.push({ "id": text, "name": admin.adname, "password": admin.adpass, "email": admin.ademail, "position": admin.adPosition });
            fs.writeFile('JSON.json', JSON.stringify(student), (err) => {
                if (err) throw err;
                console.log(student);
            });
            res.send({ message: "success", result: 1});
        }

    });

    console.log('This is after the read call');

});





module.exports = router;





