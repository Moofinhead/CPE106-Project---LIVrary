using System;

namespace LibraryMSWF.Entity
{
    public class Book
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public string BookISBN { get; set; }
        public int BookPrice { get; set; }
        public int BookCopies { get; set; }
        public bool IsAvailable => BookCopies > 0;
        public string BookImage { get; set; } = "/Images/";
        public int PublicationYear { get; set; }
        public string Genre { get; set; }
        public string Synopsis { get; set; }
        public string PdfPath { get; set; }
        public long PdfSize { get; set; } // Size in bytes
    }
}
