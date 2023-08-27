const currentDate = new Date();
const daysOfWeek = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
const months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
const dateElement = document.getElementById("currentDate");
const weekdayElement = document.querySelector(".weekday");
const monthElement = document.querySelector(".month");


dateElement.textContent = currentDate.getDate();
weekdayElement.textContent = daysOfWeek[currentDate.getDay()];
monthElement.textContent = months[currentDate.getMonth()];