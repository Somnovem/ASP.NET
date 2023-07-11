var restaurantList = document.getElementById("restaurant-list");
var address = document.getElementById("address");
var title = document.getElementById("name");
var phone = document.getElementById("phone");
var email = document.getElementById("email");
var about = document.getElementById("about");
var hours = document.getElementById("hours");
var listItems = [];
var restaurants = [];
var lastSelectedItem = null;

async function getRestaurants() {
    const response = await fetch("/api/users", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok) {
        const restaurantsJson = await response.json();
        for (var i = 0; i < restaurantsJson.length; ++i)
        {
            if (i == 0) {
                addRestaurant(restaurantsJson[i],true);
            }
            else {
                addRestaurant(restaurantsJson[i]);
            }
        }
    }
}

getRestaurants();

function addRestaurant(restaurant, isFirst = false) {
    restaurants.push(restaurant);
    var listItem = document.createElement('li');
    listItem.innerHTML = restaurant.name;
    listItem.classList.add('list-group-item');
    if (isFirst) {
        listItem.classList.add('active');
        lastSelectedItem = listItem;
        showRestaurantInfo(restaurants[0]);
    }
    listItem.addEventListener('click', (e) => { changeSelectedItem(e); });
    listItems.push(listItem);
    restaurantList.appendChild(listItem);
}

function changeSelectedItem(e) {
    if (e.target == lastSelectedItem) return;
    e.target.classList.add('active');
    lastSelectedItem.classList.remove('active');
    lastSelectedItem = e.target;
    showRestaurantInfo(restaurants[getItemIndex(e.target)]);
}

function getItemIndex(listItem) {
    for (var i = 0; i < listItems.length; ++i) {
        if (listItems[i] == listItem) return i;
    }
    return null;
}

function showRestaurantInfo(restaurant) {
    title.innerHTML = `Welcome to ${restaurant.name}`;
    address.innerHTML = `Our Address: ${restaurant.address}`;
    phone.innerHTML = `Phone Number: ${restaurant.phone}`;
    email.innerHTML = `Email: ${restaurant.email}`;
    about.innerHTML = restaurant.about;
    hours.innerHTML = restaurant.hours;
}