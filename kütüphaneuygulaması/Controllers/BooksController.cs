using kütüphaneuygulaması.Data;
using kütüphaneuygulaması.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace kütüphaneuygulaması.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search, int? categoryId, string sort)
        {
            var books = _context.Books.Include(b => b.Category).AsQueryable();

            if (!string.IsNullOrEmpty(search))
                books = books.Where(b => b.Title.Contains(search));

            if (categoryId.HasValue && categoryId > 0)
                books = books.Where(b => b.CategoryId == categoryId);

            books = sort switch
            {
                "title_asc" => books.OrderBy(b => b.Title),
                "title_desc" => books.OrderByDescending(b => b.Title),
                "price_asc" => books.OrderBy(b => b.Price),
                "price_desc" => books.OrderByDescending(b => b.Price),
                "stock_asc" => books.OrderBy(b => b.Stock),
                "stock_desc" => books.OrderByDescending(b => b.Stock),
                _ => books.OrderBy(b => b.Id)
            };

            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;
            ViewBag.Sort = sort;
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name", categoryId);

            return View(books.ToList());
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var book = _context.Books.Include(b => b.Category).FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Book newBook)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
                return View(newBook);
            }
            _context.Books.Add(newBook);
            _context.SaveChanges();
            TempData["Success"] = "Kitap başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null) return NotFound();
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name", book.CategoryId);
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Book updatedBook)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
                return View(updatedBook);
            }
            _context.Books.Update(updatedBook);
            _context.SaveChanges();
            TempData["Success"] = "Kitap başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
                TempData["Success"] = "Kitap başarıyla silindi.";
            }
            return RedirectToAction("Index");
        }
    }
}