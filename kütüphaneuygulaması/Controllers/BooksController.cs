using kütüphaneuygulaması.Data;
using kütüphaneuygulaması.Models;
using Microsoft.AspNetCore.Mvc;

namespace kütüphaneuygulaması.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        [HttpGet]
        public IActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(Book newBook)
        {
            _context.Books.Add(newBook);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var book = _context.Books.Find(id);
            return book == null ? NotFound() : View(book);
        }

        [HttpPost]
        public IActionResult Update(Book updatedBook)
        {
            _context.Books.Update(updatedBook);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}