using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;

namespace WebUi.Models
{
    public class BooksListViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public Paginginfo Paginginfo { get; set; }
        public string CurrentGenre { get; set; }
    }
}