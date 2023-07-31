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
    }
}

// Function to save edited book or add a new book
//async function saveBook() {
//    const bookId = document.getElementById("bookId").value;
//    const title = document.getElementById("title").value;
//    const description = document.getElementById("description").value;
//    const pageCount = parseInt(document.getElementById("pageCount").value);
//    const rating = parseFloat(document.getElementById("rating").value);
//    const ratingsCount = parseInt(document.getElementById("ratingsCount").value);
//    const publisher = document.getElementById("publisher").value;
//    const publishedDate = document.getElementById("publishedDate").value;
//    const categories = document.getElementById("categories").value;

//    const response = await fetch("/EditBooks?handler=EditBook", {
//        method: "PUT",
//        headers: { "Accept": "application/json", "Content-Type": "application/json" },
//        body: JSON.stringify({
//            id: parseInt(bookId, 10), // Ensure id is parsed as an integer
//            title: title,
//            description: description,
//            publisher: publisher,
//            publishedDate: publishedDate,
//            categories: categories,
//            rating: parseFloat(rating),
//            pageCount: parseInt(pageCount, 10),
//            ratingsCount: parseInt(ratingsCount, 10)
//        })
//    });

//    console.dir(response);

//    // Process the response if needed
//    const result = await response.json();
//    console.log(result);

//    document.getElementById("bookId").value = "";
//    document.getElementById("bookForm").reset();
//}


function addNewBook() {
    if (document.getElementById("bookId").value != "") saveBook();
    else
    {
        const title = document.getElementById("title").value;
        const description = document.getElementById("description").value;
        const pageCount = parseInt(document.getElementById("pageCount").value);
        const rating = parseFloat(document.getElementById("rating").value);
        const ratingsCount = parseInt(document.getElementById("ratingsCount").value);
        const publisher = document.getElementById("publisher").value;
        const publishedDate = document.getElementById("publishedDate").value;
        const categories = document.getElementById("categories").value;

        document.getElementById("bookForm").reset();
    }
}

function deleteBook(bookId)
{

}