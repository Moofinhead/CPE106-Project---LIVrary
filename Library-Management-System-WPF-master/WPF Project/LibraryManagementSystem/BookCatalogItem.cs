using System;

namespace LibraryManagementSystem
{
    public class BookCatalogItem
    {
        public string BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public bool Available { get; set; }
        public string CoverUrl { get; set; }
    }
} 