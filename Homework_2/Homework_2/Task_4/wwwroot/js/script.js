var restaurants = [
    {
        name: "Restaurant A",
        address: "123 Main Street",
        phone: "(123) 456-7890",
        email: "info@restaurantA.com",
        about: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc interdum sem et tincidunt posuere.",
        hours: "Monday - Friday: 9:00 AM - 10:00 PM"
    },
    {
        name: "Restaurant B",
        address: "456 Elm Street",
        phone: "(456) 789-0123",
        email: "info@restaurantB.com",
        about: "Sed sed felis non nunc tincidunt auctor at eget justo. Nullam aliquam pretium nisi ut rutrum.",
        hours: "Monday - Saturday: 10:00 AM - 11:00 PM"
    },
    {
        name: "Restaurant C",
        address: "789 Oak Street",
        phone: "(789) 012-3456",
        email: "info@restaurantC.com",
        about: "In lobortis tellus ac tortor placerat, et faucibus nisl pulvinar. Fusce consectetur mi ligula, ut.",
        hours: "Tuesday - Sunday: 11:00 AM - 9:00 PM"
    }
];

var restaurantList = document.getElementById("restaurant-list");
var address = document.getElementById("address");
var title = document.getElementById("name");
var phone = document.getElementById("phone");
var email = document.getElementById("email");
var about = document.getElementById("about");
var hours = document.getElementById("hours");
var listItems = [];
var lastSelectedItem = null;

for (var i = 0; i < restaurants.length; ++i) {
    var listItem = document.createElement('li');
    listItem.innerHTML = restaurants[i].name;
    listItem.classList.add('list-group-item');
    if (i == 0) {
        listItem.classList.add('active');
        lastSelectedItem = listItem;
    }
    listItem.addEventListener('click', (e) => {
        if (e.target == lastSelectedItem) return;
        e.target.classList.add('active');
        lastSelectedItem.classList.remove('active');
        lastSelectedItem = e.target;
        showRestaurantInfo(restaurants[getItemIndex(e.target)]);
    });
    listItems[i] = listItem;
    restaurantList.appendChild(listItem);
}

showRestaurantInfo(restaurants[0]);

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