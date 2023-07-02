using Bookify.Infrastructure.Persistance;
using HashidsNet;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IHashids _hashIds;
                private readonly IApplicationDBContext _context;
        private readonly IMapper _mapper;

        public SearchController(ApplicationDbContext context,
            IHashids hashIds,
            IMapper mapper)
        {
            _hashIds = hashIds;
            this._context = context;
            this._mapper = mapper;
        }
        public IActionResult Details(string id)
        {
            int unprotectedId = int.Parse(_hashIds.DecodeHex(id));
            if (unprotectedId == 0)
                return NotFound();


            var book = _context.Books.Include(b => b.Author)
                .Include(b => b.Copies)
                .Include(b => b.Categories)
                .ThenInclude(c => c.Category)
                .SingleOrDefault(b => b.Id == unprotectedId && !b.IsDeleted);

            if (book is null)
                return NotFound();

            var viewModel = _mapper.Map<BookViewModel>(book);
            return View(viewModel);
        }
        public IActionResult Index()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Books");
            }
            return View();
        }

        public IActionResult Find(string query)
        {
            var books = _context.Books
                .Include(b => b.Author)
                .Where(b => !b.IsDeleted &&
                (b.Title.Contains(query) || b.Author!.Name.Contains(query)))
                .Select(b => new { b.Title, Author = b.Author!.Name, Key = _hashIds.EncodeHex(b.Id.ToString()) })
                .ToList();

            return Ok(books);
        }
    }
}
