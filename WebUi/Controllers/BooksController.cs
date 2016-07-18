using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using WebUi.Models;

namespace WebUi.Controllers
{
    public class BooksController : Controller
    {
        private IBookRepository repository;
        public int pageSize = 4;

        public BooksController(IBookRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string genre, int page = 1)
        {
            BooksListViewModel model = new BooksListViewModel
            {
                Books = (repository.Books
                .Where(b => genre == null || b.Genre == genre)
                .OrderBy(book => book.BookId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)),
                Paginginfo = new Paginginfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = genre == null ?
                    repository.Books.Count() :
                    repository.Books.Where(book => book.Genre == genre).Count()
                },
                CurrentGenre = genre,
            };
            return View(model);
        }

        public FileContentResult GetImage(int bookId)
        {
            Book book = new Book();
            book = repository.Books.FirstOrDefault(b => b.BookId == bookId);

            if (book != null)
            {
                return File(book.ImageData, book.ImageMimeType);
            }

            else
            {
                return null;
            }
        }
    }
}