using Bookify.Domain.Consts;

namespace Bookify.Web.Core.ViewModels
{
    public class ResetPasswordFormViewModel
    {
        public string Id { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password), Display(Name = "Confirm password"),
            Compare("Password", ErrorMessage = Errors.ConfirmPasswordNotMatch)]
        public string ConfirmPassword { get; set; } = null!;
    }
}