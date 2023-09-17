function normalizeDate(dateString = "") {
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
}

async function GetUsers() {
    const response = await fetch("/api/users", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        const users = await response.json();
        let rows = document.querySelector("tbody");
        users.forEach(user => {
            rows.append(row(user));
        });
    }
}
async function GetUser(id) {
    const response = await fetch("/api/users/" + id, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        const user = await response.json();
        const form = document.forms["userForm"];
        form.elements["id"].value = user.id;
        form.elements["firstname"].value = user.firstname;
        form.elements["lastname"].value = user.lastname;
        form.elements["birthday"].value = normalizeDate(user.birthday);
    }
}

async function CreateUser(userFirstName, userLastName, userBirthday) {

    const response = await fetch("api/users", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            firstname: userFirstName,
            lastname: userLastName,
            birthday: userBirthday
        })
    });
    if (response.ok === true) {
        const user = await response.json();
        reset();
        document.querySelector("tbody").append(row(user));
    }
    else {
        const errorData = await response.json();
        console.log("errors", errorData);
        if (errorData) {
            if (errorData.errors) {
                if (errorData.errors["firstname"]) {
                    addError(errorData.errors["firstname"]);
                }
                if (errorData.errors["lastname"]) {
                    addError(errorData.errors["lastname"]);
                }
                if (errorData.errors["birthday"]) {
                    addError(errorData.errors["birthday"]);
                }
            }
            if (errorData["Firstname"]) {
                addError(errorData["Firstname"]);
            }
            if (errorData["Lastname"]) {
                addError(errorData["Lastname"]);
            }
            if (errorData["Birthday"]) {
                addError(errorData["Birthday"]);
            }
        }

        document.getElementById("errors").style.display = "block";
    }
}

async function EditUser(userId, userFirstName, userLastName, userBirthday) {
    console.log(JSON.stringify({
        id: parseInt(userId, 10),
        firstname: userFirstName,
        lastname: userLastName,
        birthday: userBirthday
    }));
    const response = await fetch("api/users", {
        method: "PUT",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            id: parseInt(userId, 10),
            firstname: userFirstName,
            lastname: userLastName,
            birthday: userBirthday
        })
    });
    if (response.ok === true) {
        const user = await response.json();
        reset();
        document.querySelector("tr[data-rowid='" + user.id + "']").replaceWith(row(user));
    }
}

async function DeleteUser(id) {
    const response = await fetch("/api/users/" + id, {
        method: "DELETE",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        const user = await response.json();
        document.querySelector("tr[data-rowid='" + user.id + "']").remove();
    }
}


function reset() {
    const form = document.forms["userForm"];
    form.reset();
    form.elements["id"].value = 0;
}
function addError(errors) {
    errors.forEach(error => {
        const p = document.createElement("p");
        p.append(error);
        document.getElementById("errors").append(p);
    });
}

function row(user) {

    const tr = document.createElement("tr");
    tr.setAttribute("data-rowid", user.id);

    const idTd = document.createElement("td");
    idTd.append(user.id);
    tr.append(idTd);

    const firstnameTd = document.createElement("td");
    firstnameTd.append(user.firstname);
    tr.append(firstnameTd);

    const lastnameTd = document.createElement("td");
    lastnameTd.append(user.lastname);
    tr.append(lastnameTd);

    const birthdayTd = document.createElement("td");
    birthdayTd.append(normalizeDate(user.birthday));
    tr.append(birthdayTd);

    const linksTd = document.createElement("td");

    const editLink = document.createElement("a");
    editLink.setAttribute("data-id", user.id);
    editLink.setAttribute("style", "cursor:pointer;padding:15px;");
    editLink.append("Change");
    editLink.addEventListener("click", e => {

        e.preventDefault();
        GetUser(user.id);
    });
    linksTd.append(editLink);

    const removeLink = document.createElement("a");
    removeLink.setAttribute("data-id", user.id);
    removeLink.setAttribute("style", "cursor:pointer;padding:15px;");
    removeLink.append("Delete");
    removeLink.addEventListener("click", e => {

        e.preventDefault();
        DeleteUser(user.id);
    });

    linksTd.append(removeLink);
    tr.appendChild(linksTd);

    return tr;
}

document.getElementById("reset").addEventListener("click", function (e) {

    e.preventDefault();
    reset();
})


document.forms["userForm"].addEventListener("submit", e => {
    e.preventDefault();
    document.getElementById("errors").innerHTML = "";
    document.getElementById("errors").style.display = "none";

    const form = document.forms["userForm"];
    const id = form.elements["id"].value;
    const firstname = form.elements["firstname"].value;
    const lastname = form.elements["lastname"].value;
    const birthday = form.elements["birthday"].value;
    if (id == 0)
        CreateUser(firstname, lastname, birthday);
    else
        EditUser(id, firstname, lastname, birthday);
});

GetUsers();