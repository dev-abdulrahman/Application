using Application.Common;
using Application.Entities.Models;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Application.Validations;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    public class BorrowController : Controller
    {
        private readonly IBooksService _booksService;
        private readonly IBorrowService _borrowService;
        private readonly IUnitOfWork _unitOfWork;
        public BorrowController(IBooksService booksService, IUnitOfWork unitOfWork, IBorrowService borrowService)
        {
            _booksService = booksService;
            _unitOfWork = unitOfWork;
            _borrowService = borrowService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int bookId)
        {
            if (bookId == null || bookId == 0)
            {
                TempData["ErrorMessage"] = "Book ID was not provided for borrowing.";
                return View(AppConstants.NotFoundView);
            }
            try
            {
                var book = await _booksService.GetBookById(bookId);
                if (book == null)
                {
                    TempData["ErrorMessage"] = $"No book found with ID {bookId} to borrow.";
                    return View(AppConstants.NotFoundView);

                }
                if (!book.IsAvailable)
                {
                    TempData["ErrorMessage"] = $"The book '{book.Title}' is currently not available for borrowing.";
                    return View(AppConstants.NotAvailableView);
                }
                var borrowViewModel = new BorrowViewModel
                {
                    BookId = book.BookId,
                    BookTitle = book.Title
                };
                //return View(borrowViewModel);
                return PartialView("~/Views/Shared/PartialViews/_BorrowCreate.cshtml", borrowViewModel);

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the borrow form.";
                return View(AppConstants.ErrorView);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BorrowViewModel model)
        {
            // manuall validation
            //BorrowValidator borrowValidator = new BorrowValidator();
            //if (!borrowValidator.Validate(model).IsValid)
            //{
            //    return View(model);
            //}

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var book = await _booksService.GetBookById(model.BookId);
                if (book == null)
                {
                    TempData["ErrorMessage"] = $"No book found with ID {model.BookId} to borrow.";
                    return View(AppConstants.NotFoundView);
                }
                if (!book.IsAvailable)
                {
                    TempData["ErrorMessage"] = $"The book '{book.Title}' is already borrowed.";
                    return View(AppConstants.NotAvailableView);
                }
                var borrowRecord = new BorrowRecord
                {
                    BookId = book.BookId,
                    BorrowerName = model.BorrowerName,
                    BorrowerEmail = model.BorrowerEmail,
                    Phone = model.Phone,
                    BorrowDate = DateTime.UtcNow
                };

                book.IsAvailable = false;
                await _booksService.UpdateBook(book);
                await _borrowService.AddBorrowedBook(borrowRecord);
                await _unitOfWork.SaveChanges();

                TempData["SuccessMessage"] = $"Successfully borrowed the book: {book.Title}.";
                //return RedirectToAction("Index", "Books");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing the borrowing action.";
                return View(AppConstants.ErrorView);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Return(int borrowRecordId)
        {
            if (borrowRecordId == null || borrowRecordId == 0)
            {
                TempData["ErrorMessage"] = "Borrow Record ID was not provided for returning.";
                return View(AppConstants.NotFoundView);
            }
            try
            {
                var borrowRecord = await _borrowService.GetBorrowedBookById(borrowRecordId);
                if (borrowRecord == null)
                {
                    TempData["ErrorMessage"] = $"No borrow record found with ID {borrowRecordId} to return.";
                    return View(AppConstants.NotFoundView);
                }
                if (borrowRecord.ReturnDate != null)
                {
                    TempData["ErrorMessage"] = $"The borrow record for '{borrowRecord.Book.Title}' has already been returned.";
                    return View(AppConstants.AlreadyReturnedView);
                }
                var returnViewModel = new ReturnViewModel
                {
                    BorrowRecordId = borrowRecord.BorrowRecordId,
                    BookTitle = borrowRecord.Book.Title,
                    BorrowerName = borrowRecord.BorrowerName,
                    BorrowDate = borrowRecord.BorrowDate
                };
                //return View(returnViewModel);
                return PartialView("~/Views/Shared/PartialViews/_BorrowReturn.cshtml", returnViewModel);

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the return confirmation.";
                return View(AppConstants.ErrorView);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(ReturnViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var borrowRecord = await _borrowService.GetBorrowedBookById(model.BorrowRecordId);
                if (borrowRecord == null)
                {
                    TempData["ErrorMessage"] = $"No borrow record found with ID {model.BorrowRecordId} to return.";
                    return View(AppConstants.NotFoundView);
                }
                if (borrowRecord.ReturnDate != null)
                {
                    TempData["ErrorMessage"] = $"The borrow record for '{borrowRecord.Book.Title}' has already been returned.";
                    return View(AppConstants.AlreadyReturnedView);
                }
                
                borrowRecord.ReturnDate = DateTime.UtcNow;
                borrowRecord.Book.IsAvailable = true;

                await _borrowService.UpdateBorrowedBook(borrowRecord);
                await _unitOfWork.SaveChanges();

                TempData["SuccessMessage"] = $"Successfully returned the book: {borrowRecord.Book.Title}.";
                //return RedirectToAction("Index", "Books");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing the return action.";
                return View(AppConstants.ErrorView);
            }
        }
    }
}
