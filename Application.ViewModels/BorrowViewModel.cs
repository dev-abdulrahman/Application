using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.ViewModels
{
    public class BorrowViewModel
    {
        public int BookId { get; set; }

        [BindNever]
        public string? BookTitle { get; set; }

        public string? BorrowerName { get; set; }

        public string? BorrowerEmail { get; set; }

        public string? Phone { get; set; }
    }
}
