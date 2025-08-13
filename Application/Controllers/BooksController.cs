using Application.Common;
using Application.Entities.Models;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBooksService _booksService;
        private readonly IUnitOfWork _unitOfWork;

        public BooksController(IBooksService booksService, IUnitOfWork unitOfWork)
        {
            _booksService = booksService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var books = await _booksService.GetAllBooks();
                return View(books);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the books.";
                return View(AppConstants.ErrorView);
            }
        }

        public async Task<IActionResult> GetBooksTable()
        {
            try
            {
                var books = await _booksService.GetAllBooks();
                return PartialView("~/Views/Shared/PartialViews/_BooksTable.cshtml", books);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error loading books table.");
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0)
            {
                TempData["ErrorMessage"] = "Book ID was not provided.";
                return View(AppConstants.NotFoundView);
            }
            try
            {
                var book = await _booksService.GetBookById((int)id);
                if (book == null)
                {
                    TempData["ErrorMessage"] = $"No book found with ID {id}.";
                    return View(AppConstants.NotFoundView);
                }
                //return View(book);
                return PartialView("~/Views/Shared/PartialViews/_BooksDetails.cshtml", book);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the book details.";
                return View(AppConstants.ErrorView);
            }
        }

        public IActionResult Create()
        {
            //return View();
            return PartialView("~/Views/Shared/PartialViews/_BooksCreate.cshtml");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _booksService.AddBook(book);
                    await _unitOfWork.SaveChanges();

                    TempData["SuccessMessage"] = $"{AppConstants.BookAddedSuccess}: {book.Title}.";
                    //return RedirectToAction(nameof(Index));
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occurred while adding the book.";
                    return View(book);
                }
            }
            //return View(book);
            return PartialView("~/Views/Shared/PartialViews/_BooksCreate.cshtml", book);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                TempData["ErrorMessage"] = "Book ID was not provided for editing.";
                return View(AppConstants.NotFoundView);
            }
            try
            {
                var book = await _booksService.GetBookById((int)id);
                if (book == null)
                {
                    TempData["ErrorMessage"] = $"No book found with ID {id} for editing.";
                    return View(AppConstants.NotFoundView);
                }
                //return View(book);
                return PartialView("~/Views/Shared/PartialViews/_BooksEdit.cshtml", book);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the book for editing.";
                return View(AppConstants.ErrorView);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Book book)
        {
            if (id == null || id == 0)
            {
                TempData["ErrorMessage"] = "Book ID was not provided for updating.";
                return View(AppConstants.NotFoundView);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var existingBook = await _booksService.GetBookById((int)id);
                    if (existingBook == null)
                    {
                        TempData["ErrorMessage"] = $"No book found with ID {id} for updating.";
                        return View(AppConstants.NotFoundView);
                    }

                    existingBook.Title = book.Title;
                    existingBook.Author = book.Author;
                    existingBook.ISBN = book.ISBN;
                    existingBook.PublishedDate = book.PublishedDate;

                    await _booksService.UpdateBook(existingBook);
                    await _unitOfWork.SaveChanges();

                    TempData["SuccessMessage"] = $"Successfully updated the book: {book.Title}.";
                    //return RedirectToAction(nameof(Index));
                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    TempData["ErrorMessage"] = "A concurrency error occurred during the update.";
                    return View(AppConstants.ErrorView);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occurred while updating the book.";
                    return View(AppConstants.ErrorView);
                }
            }
            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                TempData["ErrorMessage"] = "Book ID was not provided for deletion.";
                return View(AppConstants.NotFoundView);
            }
            try
            {
                var book = await _booksService.GetBookById((int)id);
                if (book == null)
                {
                    TempData["ErrorMessage"] = $"No book found with ID {id} for deletion.";
                    return View(AppConstants.NotFoundView);
                }
                //return View(book);
                return PartialView("~/Views/Shared/PartialViews/_BooksDelete.cshtml", book);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the book for deletion.";
                return View(AppConstants.ErrorView);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var book = await _booksService.GetBookById(id);
                if (book == null)
                {
                    TempData["ErrorMessage"] = $"No book found with ID {id} for deletion.";
                    return View(AppConstants.NotFoundView);
                }
                await _booksService.DeleteBook(id);
                await _unitOfWork.SaveChanges();

                TempData["SuccessMessage"] = $"Successfully deleted the book: {book.Title}.";
                //return RedirectToAction(nameof(Index));
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the book.";
                return View(AppConstants.ErrorView);
            }
        }
    }
}
