using HashidsNet;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Bookify.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHashids _hashIds;

        public HomeController(ApplicationDbContext context, IMapper mapper, IHashids hashIds)
        {
            this._context = context;
            this._mapper = mapper;
            _hashIds = hashIds;
        }

        public IActionResult Index()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            var lastAddedBooks = _context.Books.Include(b => b.Author).Where(b => !b.IsDeleted).OrderByDescending(b => b.Id).Take(10).ToList();
            var viewModel = _mapper.Map<IEnumerable<BookViewModel>>(lastAddedBooks);

            foreach (var book in viewModel)
                book.Key = _hashIds.EncodeHex(book.Id.ToString());
            
            
            return View(viewModel);
        }
        public IActionResult Error(int statusCode = 500)
        {
            return View(new ErrorViewModel
            {
                ErrorCode = statusCode,
                ErrorDescription = ReasonPhrases.GetReasonPhrase(statusCode)
            }) ;
        }
    }
}