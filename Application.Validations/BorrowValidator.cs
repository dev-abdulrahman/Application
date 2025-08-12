using Application.ViewModels;
using FluentValidation;

namespace Application.Validations
{
    public class BorrowValidator : AbstractValidator<BorrowViewModel>
    {
        public BorrowValidator() 
        {
            RuleFor(x => x.BookId)
                .NotEmpty().NotNull();

            RuleFor(x => x.BorrowerName)
                .NotEmpty().WithMessage("Name is required.").NotNull()
                .Length(3, 100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.BorrowerEmail)
                .NotEmpty().WithMessage("Email is required.").NotNull()
                .EmailAddress();

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required.").NotNull()
                .Matches(@"^(?:\+91[\-\s]?|91[\-\s]?)?[6-9]\d{9}$")
                .WithMessage("Invalid phone number.");
        }
    }
}
