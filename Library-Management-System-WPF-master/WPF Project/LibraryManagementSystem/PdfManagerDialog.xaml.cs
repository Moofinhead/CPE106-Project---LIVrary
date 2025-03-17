using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using LibraryMSWF.Entity;
using LibraryMSWF.DAL;

namespace LibraryManagementSystem
{
    public partial class PdfManagerDialog : Window
    {
        private readonly Book _book;
        private readonly BookDAL _bookDAL;
        private string _selectedPdfPath;
        private const long MaxPdfSize = 52428800; // 50MB in bytes

        public PdfManagerDialog(Book book)
        {
            InitializeComponent();
            _book = book;
            _bookDAL = new BookDAL();
            LoadBookDetails();
        }

        private void LoadBookDetails()
        {
            BookTitle.Text = $"Managing PDF for: {_book.BookName}";
            
            if (!string.IsNullOrEmpty(_book.PdfPath) && File.Exists(_book.PdfPath))
            {
                CurrentFile.Text = Path.GetFileName(_book.PdfPath);
                FileSize.Text = FormatFileSize(_book.PdfSize);
            }
            else
            {
                CurrentFile.Text = "No PDF file attached";
                FileSize.Text = "N/A";
            }
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                Title = "Select a PDF file"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var fileInfo = new FileInfo(openFileDialog.FileName);
                
                if (fileInfo.Length > MaxPdfSize)
                {
                    MessageBox.Show("Selected file is too large. Maximum size is 50MB.", 
                                  "File Too Large", 
                                  MessageBoxButton.OK, 
                                  MessageBoxImage.Warning);
                    return;
                }

                _selectedPdfPath = openFileDialog.FileName;
                SelectedFileText.Text = $"Selected: {Path.GetFileName(_selectedPdfPath)}";
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedPdfPath))
            {
                DialogResult = true;
                Close();
                return;
            }

            try
            {
                bool success = _bookDAL.UploadPdf(_book.BookId, _selectedPdfPath);
                
                if (success)
                {
                    MessageBox.Show("PDF file updated successfully!", 
                                  "Success", 
                                  MessageBoxButton.OK, 
                                  MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Failed to update PDF file. Please try again.", 
                                  "Error", 
                                  MessageBoxButton.OK, 
                                  MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating PDF: {ex.Message}", 
                              "Error", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double size = bytes;
            
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size = size / 1024;
            }

            return $"{size:0.##} {sizes[order]}";
        }
    }
} 