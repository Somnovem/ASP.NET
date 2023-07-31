var books = ""

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

function editBook(bookId) {
    const book = books.find(book => book.Id == bookId);
    if (book) {
        document.getElementById("bookId").value = book.Id;
        document.getElementById("title").value = book.Title;
        document.getElementById("description").value = book.Description;
        document.getElementById("pageCount").value = book.PageCount;
        document.getElementById("rating").value = book.AverageRating;
        document.getElementById("ratingsCount").value = book.RatingsCount;
        document.getElementById("publisher").value = book.Publisher;
        document.getElementById("publishedDate").value = book.PublishedDate;
        document.getElementById("categories").value = book.Categories;
        document.getElementById("authors").value = book.Authors;
    }
}

function deleteBook(id) {

    if (window.confirm('Do you really wanna delete this book?'))
    {
        var jsonDataUrl = `?handler=Delete&id=${id}`;
        fetch(jsonDataUrl).then(() => {
            window.location.reload();
        }
        );
    }
}