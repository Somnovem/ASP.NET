var phoneRegex = /^\+?\(?\d{2}\)?[ -]?(?:\d{3}[ -]?){2}\d{4}/;
var phoneInput = document.getElementById('form-phone-input');
phoneInput.addEventListener('input', () => {
    phoneInput.classList.remove('is-valid');
    phoneInput.classList.remove('is-invalid');
    if (phoneRegex.test(phoneInput.value)) phoneInput.classList.add('is-valid');
    else phoneInput.classList.add('is-invalid');
});

// ѕолучение всех пользователей
async function getUsers() {
    // отправл€ет запрос и получаем ответ
    const response = await fetch("/api/users", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    // если запрос прошел нормально
    if (response.ok) {
        // получаем данные
        const users = await response.json();
        const rows = document.querySelector("tbody");
        // добавл€ем полученные элементы в таблицу
        users.forEach(user => rows.append(row(user)));
    }
}
// ѕолучение одного пользовател€
async function getUser(id) {
    const response = await fetch("/api/users/" + id, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok) {
        const user = await response.json();
        const form = document.forms["userForm"];
        form.elements["id"].value = user.id;
        form.elements["name"].value = user.name;
        form.elements["age"].value = user.age;
        form.elements["phone"].value = user.phone;
    }
    else {
        // если произошла ошибка, получаем сообщение об ошибке
        const error = await response.json();
        console.log(error.message); // и выводим его на консоль
    }
}
// ƒобавление пользовател€
async function createUser(userName,userPhone, userAge) {

    const response = await fetch("api/users", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            name: userName,
            phone: userPhone,
            age: parseInt(userAge, 10)
        })
    });
    if (response.ok) {
        const user = await response.json();
        reset();
        document.querySelector("tbody").append(row(user));
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}
// »зменение пользовател€
async function editUser(userId, userName,userPhone, userAge) {
    const response = await fetch("api/users", {
        method: "PUT",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            id: userId,
            name: userName,
            phone: userPhone,
            age: parseInt(userAge, 10)
        })
    });
    if (response.ok) {
        const user = await response.json();
        reset();
        document.querySelector("tr[data-rowid='" + user.id + "']").replaceWith(row(user));
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}
// ”даление пользовател€
async function deleteUser(id) {
    const response = await fetch("/api/users/" + id, {
        method: "DELETE",
        headers: { "Accept": "application/json" }
    });
    if (response.ok) {
        const user = await response.json();
        document.querySelector("tr[data-rowid='" + user.id + "']").remove();
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}

// сброс данных формы после отправки
function reset() {
    phoneInput.classList.remove('is-valid');
    phoneInput.classList.remove('is-invalid');
    const form = document.forms["userForm"];
    form.reset();
    form.elements["id"].value = 0;
}
// создание строки дл€ таблицы
function row(user) {

    const tr = document.createElement("tr");
    tr.setAttribute("data-rowid", user.id);

    const nameTd = document.createElement("td");
    nameTd.classList.add('text-center');
    nameTd.append(user.name);
    tr.append(nameTd);

    const ageTd = document.createElement("td");
    ageTd.classList.add('text-center');
    ageTd.append(user.age);
    tr.append(ageTd);

    const phoneTd = document.createElement("td");
    phoneTd.classList.add('text-center');
    phoneTd.append(user.phone);
    tr.append(phoneTd);

    const linksTd = document.createElement("td");
    linksTd.setAttribute('style','display:flex;justify-content:center;')

    const editLink = document.createElement("a");
    editLink.classList.add('user-option-button')
    editLink.append("Change");
    editLink.addEventListener("click", e => {

        e.preventDefault();
        getUser(user.id);
    });
    linksTd.append(editLink);

    const removeLink = document.createElement("a");
    removeLink.classList.add('user-option-button');
    removeLink.classList.add('btn-danger');
    removeLink.append("Delete");
    removeLink.addEventListener("click", e => {
        e.preventDefault();
        if (window.confirm('Delete the record?')) deleteUser(user.id);
    });

    linksTd.append(removeLink);
    tr.appendChild(linksTd);

    return tr;
}
// сброс значений формы
document.getElementById("reset").addEventListener("click", e => {

    e.preventDefault();
    reset();
})

// отправка формы
document.forms["userForm"].addEventListener("submit", e => {
    e.preventDefault();
    const form = document.forms["userForm"];
    const id = form.elements["id"].value;
    const name = form.elements["name"].value;
    const phone = form.elements["phone"].value;
    const age = form.elements["age"].value;
    if (id == 0)
        createUser(name, phone, age);
    else
        editUser(id, name, phone, age);
});

// загрузка пользователей
getUsers();