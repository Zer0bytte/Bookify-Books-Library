namespace Bookify.Web.Validators
{
    public class BookCopyValidator: AbstractValidator<BookCopyFormViewModel>
    {
        public BookCopyValidator()
        {
            RuleFor(c => c.EditionNumber).InclusiveBetween(1, 1000);
        }
    }
}
