var books = "";
const booksContainer = document.getElementById("book-list");
function fetchBooks() {
  var jsonDataUrl = "?handler=JsonData";
  fetch(jsonDataUrl)
    .then((response) => response.json())
    .then((jsonData) => {
      books = JSON.parse(jsonData);
    })
    .catch((error) => {
      console.error("Error fetching JSON data:", error);
    });
}

fetchBooks();

function displayBooks(items) {
  booksContainer.innerHTML = "";
  for (var i = 0; i < items.length; i++) {
      var bookCard = ` <div class="col-md-4 book-card">
                       <img src=${items[i].ThumbnailLink} alt=${items[i].Title} class="book-thumbnail" style="width:125px;height:225px;">
                     `;
      if (items[i].Title.Length > 20)
      {
          bookCard += `<div class="book-title">Title: ${items[i].Title.substring(0,20)}...</div>`;
      }
      else
      {
          bookCard += `<div class="book-title">Title: ${items[i].Title}...</div>`;
      }
    bookCard += `
					<div class="book-author">Author(s): ${items[i].Authors}</div>
					<div class="book-stats">Rating: ${items[i].AverageRating}</div>
					<div class="book-stats">Ratings count: ${items[i].RatingsCount}</div>
					<div class="book-stats">Page count: ${items[i].PageCount}</div>
					<div class="book-stats">Category(s): ${items[i].Categories}</div>`;

      if (items[i].Publisher != null) {
      bookCard += `<div class="book-stats">Publisher: ${items[i].Publisher}</div>`;
    }
    bookCard += `
					<div class="book-stats">Date published: ${items[i].PublishedDate}</div>
					<a href="/Details/${items[i].Id}" class="btn btn-primary mt-3">View Details</a>
			</div>
        `;
    booksContainer.innerHTML += bookCard;
  }
}


function filterItemsByName(searchTerm) {
    return books.filter((item) =>
        item.Title.toLowerCase().includes(searchTerm.toLowerCase())
    );
}

function filterItemsByCategory(categoryValue) {
    return books.filter((item) =>
        item.Categories.includes(categoryValue)
    );
}

document.getElementById("searchInput").addEventListener("input", (event) => {
    const searchTerm = event.target.value;
    const filteredItems = filterItemsByName(searchTerm);
    displayBooks(filteredItems);
});

document.getElementById("categorySelect").addEventListener("change", (event) => {
    const categoryValue = event.target.value;
    const filteredItems = (categoryValue == "") ? books : filterItemsByCategory(categoryValue);
    displayBooks(filteredItems);
});

document.getElementById("sortPropertyAsc").addEventListener("click", () => {
    const books = sortItemsByPropertyAsc(document.getElementById("sortingSelect").value);
    displayBooks(books);
});

document.getElementById("sortPropertyDesc").addEventListener("click", () => {
    const books = sortItemsByPropertyDesc(document.getElementById("sortingSelect").value);
    displayBooks(books);
});

function sortItemsByPropertyAsc(propertyName = "") {
    console.dir(books.slice().sort((a, b) => a[propertyName] - b[propertyName]));
    return books.slice().sort((a, b) => a[propertyName] - b[propertyName]);
}

function sortItemsByPropertyDesc(propertyName = "") {
    return books.slice().sort((a, b) => b[propertyName] - a[propertyName]);
}