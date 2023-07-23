const searchButton = document.getElementById("searchButton");
searchButton.addEventListener("click", searchBooks);

async function searchBooks() {
    const searchTerm = document.getElementById("searchInput").value.trim();
    if (searchTerm === "") {
        return;
    }
    searchButton.setAttribute("disabled", "disabled");
    try {
        const response = await fetch(`?handler=SearchBooks&searchTerm=${encodeURIComponent(searchTerm)}`);
        const books = await response.json();
        displayResults(books);
    } catch (error) {
        console.error("Error fetching data:", error);
    }
}

function displayResults(books) {
    const resultsContainer = document.getElementById("results");
    resultsContainer.innerHTML = "";

    if (!books || books.length === 0) {
        resultsContainer.innerHTML = "<p>No books found.</p>";
        searchButton.removeAttribute("disabled");
        return;
    }
    books.forEach((book) => {
        const bookItem = document.createElement("div");
        bookItem.classList.add("book-item");

        const id = book.id;
        const title = book.title;
        const authors = book.authors.join(", ");
        const categories = book.categories.join(", ");
        const previewLink = book.previewLink;
        const smallThumbnail = book.smallThumbnail;
        const publisher = book.publisher;
        const publishedDate = book.publishedDate;

        bookItem.innerHTML = `<div class="d-flex align-items-center mb-3">
                            <h3 class="me-auto">${title}</h3>
                            <img class="thumbnail" src="${smallThumbnail}" alt="${title}">
                          </div>
                          <p class="book-info"><strong>Author(s): </strong>${authors}</p>
                          <p class="book-info"><strong>Categories: </strong>${categories}</p>
                          <p class="book-info"><strong>Published By: </strong>${publisher}</p>
                          <p class="book-info"><strong>Published Date: </strong>${publishedDate}</p>
                          <div class="mt-3 d-flex justify-content-between">
                            <a class="btn btn-primary" href="${previewLink}" target="_blank">Preview</a>
                            <a class="btn btn-secondary" href="details/${id}">More Details</a>
                          </div>`;

        resultsContainer.appendChild(bookItem);
    });
    searchButton.removeAttribute("disabled");
}
