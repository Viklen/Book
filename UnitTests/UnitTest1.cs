using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using WebUi.Controllers;
using WebUi.HtmlHelpers;
using WebUi.Models;


namespace UnitTests
{
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //Организация
            Mock<IBookRepository>mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book {BookId = 1, Name = "Book1"},
                new Book {BookId = 2, Name = "Book2"},
                new Book {BookId = 3, Name = "Book3"},
                new Book {BookId = 4, Name = "Book4"},
                new Book {BookId = 5, Name = "Book5"},
            });
            BooksController controller = new BooksController(mock.Object);
            controller.pageSize = 3;

            //Действие
            BooksListViewModel result = (BooksListViewModel) controller.List(null,2).Model;
            
            //Утверждение
            List<Book> books = result.Books.ToList();
            Assert.IsTrue(books.Count == 2);
            Assert.AreEqual(books[0].Name ,"Books4");
            Assert.AreEqual(books[1].Name, "Books5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Организация
            HtmlHelper myHelper = null;
            Paginginfo paginginfo = new Paginginfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //Действие
            MvcHtmlString result = myHelper.PageLinks(paginginfo, pageUrlDelegate);

            //Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a"
                            + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a"
                            + @"<a class=""btn btn-default"" href=""Page3"">3</a", 
                            result.ToString());

        }

        [TestMethod]

        public void Can_Send_Pagination_View_Model()
        {
            //Организация
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book {BookId = 1, Name = "Book1"},
                new Book {BookId = 2, Name = "Book2"},
                new Book {BookId = 3, Name = "Book3"},
                new Book {BookId = 4, Name = "Book4"},
                new Book {BookId = 5, Name = "Book5"},
            });
            BooksController controller = new BooksController(mock.Object);
            controller.pageSize = 3;

            //Действие
            BooksListViewModel result = (BooksListViewModel)controller.List(null,2).Model;

            Paginginfo paginginfo = result.Paginginfo;
            Assert.AreEqual(paginginfo.CurrentPage, 2);
            Assert.AreEqual(paginginfo.CurrentPage, 3);
            Assert.AreEqual(paginginfo.CurrentPage, 5);
            Assert.AreEqual(paginginfo.CurrentPage, 2);
        }

        [TestMethod]

        public void Can_Filter_Books()
        {
            //Организация
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book {BookId = 1, Name = "Book1", Genre = "Genre1"},
                new Book {BookId = 2, Name = "Book2", Genre = "Genre2"},
                new Book {BookId = 3, Name = "Book3", Genre = "Genre1"},
                new Book {BookId = 4, Name = "Book4", Genre = "Genre3"},
                new Book {BookId = 5, Name = "Book5", Genre = "Genre2"},
            });
            BooksController controller = new BooksController(mock.Object);
            controller.pageSize = 3;

            //Действие
            List<Book> result = ((BooksListViewModel)controller.List("Genre2", 2).Model).Books.ToList();

            Assert.AreEqual(result.Count, 2);
            Assert.IsTrue(result[0].Name == "Books2" && result[0].Genre == "Genre2");
            Assert.IsTrue(result[1].Name == "Books5" && result[1].Genre == "Genre2");
        }

        [TestMethod]

        public void Can_Create_Categories()
        {
            //Организация
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book {BookId = 1, Name = "Book1", Genre = "Genre1"},
                new Book {BookId = 2, Name = "Book2", Genre = "Genre2"},
                new Book {BookId = 3, Name = "Book3", Genre = "Genre1"},
                new Book {BookId = 4, Name = "Book4", Genre = "Genre3"},
                new Book {BookId = 5, Name = "Book5", Genre = "Genre2"},
            });
            NavController target = new NavController(mock.Object);

            //Действие
            List<string> result = ((IEnumerable<string>)target.Menu().Model).ToList();

            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual(result[0], "Genre1");
            Assert.AreEqual(result[1], "Genre2");
            Assert.AreEqual(result[2], "Genre3");
        }

        [TestMethod]

        public void Indicates_Selected_Genre()
        {
            //Организация
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book {BookId = 1, Name = "Book1", Genre = "Genre1"},
                new Book {BookId = 2, Name = "Book2", Genre = "Genre2"},
                new Book {BookId = 3, Name = "Book3", Genre = "Genre1"},
                new Book {BookId = 4, Name = "Book4", Genre = "Genre3"},
                new Book {BookId = 5, Name = "Book5", Genre = "Genre2"},
            });
            NavController target = new NavController(mock.Object);
            string genreToSelect = "Genre2";

            //Действие
            string result = target.Menu(genreToSelect).ViewBag.SelectedGenre;
            Assert.AreEqual(genreToSelect, result);
           
        }

        [TestMethod]

        public void Generete_Genre_Specific_Book_Count()
        {
            //Организация
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book {BookId = 1, Name = "Book1", Genre = "Genre1"},
                new Book {BookId = 2, Name = "Book2", Genre = "Genre2"},
                new Book {BookId = 3, Name = "Book3", Genre = "Genre1"},
                new Book {BookId = 4, Name = "Book4", Genre = "Genre3"},
                new Book {BookId = 5, Name = "Book5", Genre = "Genre2"},
            });
            BooksController controller = new BooksController(mock.Object);
            controller.pageSize = 3;

            int res1 = ((BooksListViewModel) controller.List("Genre1").Model).Paginginfo.TotalItems;
            int res2 = ((BooksListViewModel)controller.List("Genre2").Model).Paginginfo.TotalItems;
            int res3 = ((BooksListViewModel)controller.List("Genre3").Model).Paginginfo.TotalItems;
            int resAll = ((BooksListViewModel)controller.List(null).Model).Paginginfo.TotalItems;

            Assert.AreEqual(res1,2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
