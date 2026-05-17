namespace kütüphaneuygulaması.Models
{
    public class BookRepository
    {
        private static List<Book> _books = new List<Book>
        {
            private static List<Book> _books = new List<Book>();
    };

        public List<Book> GetAll()
        {
            return _books;
        }

        public void Add(Book newBook)
        {
            newBook.Id = _books.Count > 0 ? _books.Max(b => b.Id) + 1 : 1;
            _books.Add(newBook);
        }

        public void Remove(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book != null)
                _books.Remove(book);
        }

        public Book? GetById(int id)
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }

        public void Update(Book updatedBook)
        {
            var book = _books.FirstOrDefault(b => b.Id == updatedBook.Id);
            if (book != null)
            {
                book.Title = updatedBook.Title;
                book.Author = updatedBook.Author;
                book.Price = updatedBook.Price;
                book.Stock = updatedBook.Stock;
            }
        }
    }
}