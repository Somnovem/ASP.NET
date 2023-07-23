var storeItems = '';

function fetchStoreItems()
{
    var jsonDataUrl = "?handler=JsonData";
    fetch(jsonDataUrl)
        .then(response => response.json())
        .then(jsonData => {
            storeItems = JSON.parse(jsonData);
        })
        .catch(error => {
            console.error("Error fetching JSON data:", error);
        });
}

fetchStoreItems();

function displayStoreItems(items) {
    const storeItemsContainer = document.getElementById("storeItems");
    storeItemsContainer.innerHTML = "";
    for (var i = 0; i < items.length; i++) {
        const itemCard = `
       <div class="col-lg-4 col-md-6 mb-4">
                <div class="card h-100">
                    <img class="card-img-top" src=${items[i].ThumbnailLink} alt=${items[i].Name}>
                    <div class="card-body">
                        <h4 class="card-info card-title">${items[i].Name}</h4>
                        <h4 class="card-info card-discount">Discount: ${items[i].DiscountPercentage}%</h4>
                        <h5 class="card-info card-price">Price: $${items[i].Price}</h5>
                        <h5 class="card-info card-stock">Stock: ${items[i].Stock}</h5>
                        <div class="rating">
                                ${getRatingHtml(items[i])}
                        </div>
                    </div>
                </div>
       </div>
    `;
        storeItemsContainer.innerHTML += itemCard;
    }
}

function getRatingHtml(product)
{
    var ratingHtml = '';
    for (var i = 0; i < product.Rating; i++) {
        ratingHtml += '<span class="star">&#9733;</span>';
    }
    for (var i = 0; i < 5 - product.Rating; i++) {
        ratingHtml += '<span class="star empty">&#9733;</span>';
    }
    return ratingHtml;
}

function filterItemsByName(searchTerm) {
    return storeItems.filter((item) =>
        item.Name.toLowerCase().includes(searchTerm.toLowerCase())
    );
}

function filterItemsByCategory(categoryValue) {
    return storeItems.filter((item) =>
        item.Category == categoryValue
    );
}

document.getElementById("searchInput").addEventListener("input", (event) => {
    const searchTerm = event.target.value;
    const filteredItems = filterItemsByName(searchTerm);
    displayStoreItems(filteredItems);
});

document.getElementById("categorySelect").addEventListener("change", (event) =>
{
    const categoryValue = event.target.value;
    const filteredItems = (categoryValue == "") ? storeItems : filterItemsByCategory(categoryValue);
    displayStoreItems(filteredItems);
});

document.getElementById("sortPropertyAsc").addEventListener("click", () => {
    const sortedItems = sortItemsByPropertyAsc(document.getElementById("sortingSelect").value);
    displayStoreItems(sortedItems);
});

document.getElementById("sortPropertyDesc").addEventListener("click", () => {
    const sortedItems = sortItemsByPropertyDesc(document.getElementById("sortingSelect").value);
    displayStoreItems(sortedItems);
});

function sortItemsByPropertyAsc(propertyName = "") {
    return storeItems.slice().sort((a, b) => a[propertyName] - b[propertyName]);
}

function sortItemsByPropertyDesc(propertyName = "") {
    return storeItems.slice().sort((a, b) => b[propertyName] - a[propertyName]);
}