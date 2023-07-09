namespace Bookify.Web.Validators;
public class BookValidator : AbstractValidator<BookFormViewModel>
{
    public BookValidator()
    {
        RuleFor(x => x.Title).MaximumLength(500);
        RuleFor(x => x.Publisher).MaximumLength(200);
        RuleFor(x => x.Hall).MaximumLength(50);

    }
}
