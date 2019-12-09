const Reguri = 'api/Auth/AddUser';
const loguri = 'api/Auth/authenticate';
const useruri = 'api/Auth/GetUser';


let todos = [];

function addItem() {


    const first = document.getElementById('Firstname');
    const last = document.getElementById('LastName');
    const email = document.getElementById('Email');
    const password = document.getElementById('Password');
    const confirm = document.getElementById('confirm');
    const middle = document.getElementById('MiddleName');

     var resp = validatePassword(password, confirm);

    if (resp === true) {
        return;
    }

    const item = {
        FirstName: first.value.trim(),
        LastName: last.value.trim(),
        Email: email.value.trim(),
        Password: password.value.trim(),
        ConfirmPassword: confirm.value.trim(),
        MiddleName: middle.value.trim()
    };

    fetch(Reguri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(function (response) {
            if (response.status === 400) {
                response.json().then(function (object) {
                    alert(object.message);
                });
            } else if (response.status === 200) {
                response.json().then(function (object) {
                    _displayItems(object);
                });
            }
        });
}

function login() {

    
    const email = document.getElementById('Email');
    const password = document.getElementById('Password');
   
    const item = {
        Email: email.value.trim(),
        Password: password.value.trim(),
    };

    fetch(loguri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(function (response) {
            if (response.status === 400) {
                response.json().then(function (object) {
                    alert(object.message);
                });
            } else if (response.status === 200) {
                response.json().then(function (object) {
                    _displayItems2(object);
                });
            }
        });
}

function _displayItems(data) {

    alert(data);
    window.location.href = "login.html";
}

function _displayItems2(data) {

    sessionStorage.setItem('user', data.token);

    window.location.href = "index.html";
}

function _displayProfile(data) {

    document.getElementById('Firstname').value = data.firstName;
    document.getElementById('MiddleName').value = data.middleName;
    document.getElementById('LastName').value = data.lastName;
    document.getElementById('Email').value = data.email;

}

function getProfile() {

    const token = sessionStorage.getItem('user');

    fetch(useruri, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'bearer ' + token
        }
    })
        .then(function (response) {
            if (response.status === 401) {
                // do what you need to do here
                alert("you have to login to access this page");
                window.location.href = "login.html";

            }
            else {
                response.json().then(function (object) {
                    _displayProfile(object);
                });
            }

        })
        .catch(function (error) {
            console.log('DO WHAT YOU WANT');
        });
        //.then(function (response) {
        //    if (response.status === 401) {
        //        response.json().then(function (object) {
        //            //_displayProfile(object);
        //            alert("401");
        //        });
        //    } else
        //        {
        //        response.json().then(function (object) {
        //            alert("You have to login to access this page");
        //            window.location.href = "index.html";
        //        });
        //    }
        //});
        //.then(response => response.json())
        //.then(data => _displayProfile(data))

        //.catch(error => console.error('Unable to add item.', error));


}

function validatePassword(password,confirm) {
    if (password.value !== confirm.value) {
        alert("Passwords Don't Match");
        return true;
    } else {
        return false;   }
}

function displayError(data) {
    alert(data);   
    return;
}

function logout() {
    sessionStorage.clear();
    window.location.href = "login.html";
}
