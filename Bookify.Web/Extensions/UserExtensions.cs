using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bookify.Web.Extensions
{
    public static class UserExtensions
    {
        public static string GetUserId(this ClaimsPrincipal User)
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
