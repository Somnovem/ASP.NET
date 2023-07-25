"use strict";

const body = document.body;
const menu = body.querySelector(".menu");
const menuItems = menu.querySelectorAll(".menu__item");
const menuBorder = menu.querySelector(".menu__border");
let activeItem = null;

function clickItem(item, index) {
    menu.style.removeProperty("--timeOut");
    if (activeItem == item) return;
    if (activeItem) {
        activeItem.classList.remove("active");
    }
    item.classList.add("active");
    activeItem = item;
    offsetMenuBorder(activeItem, menuBorder);
}

function offsetMenuBorder(element, menuBorder) {
    const offsetActiveItem = element.getBoundingClientRect();
    const left = Math.floor(offsetActiveItem.left - menu.offsetLeft - (menuBorder.offsetWidth - offsetActiveItem.width) / 2) + "px";
    menuBorder.style.transform = `translate3d(${left}, 0 , 0)`;
}


var pageIndex = -1;
if (document.title == 'Home') pageIndex = 0;
else if (document.title == 'Store') pageIndex = 1;
else if (document.title == 'Details') pageIndex = 1;
else if (document.title == 'Message') pageIndex = 2;
else if (document.title == 'Authorise') pageIndex = 3;

menuItems.forEach((item, index) => {
    if (index == pageIndex) item.classList.add('active');
    item.addEventListener("click", () => clickItem(item, index));
})

activeItem = document.getElementsByClassName('active')[0];
offsetMenuBorder(activeItem, menuBorder);

window.addEventListener("resize", () => {
    offsetMenuBorder(activeItem, menuBorder);
    menu.style.setProperty("--timeOut", "none");
});