using Bookify.Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
                private readonly IApplicationDBContext _context;
        private readonly IMapper _mapper;

        public DashboardController(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        public IActionResult Index()
        {
            int numberOfCopies = _context.Books.Count(c => !c.IsDeleted);
            int numberOfSubscribers = _context.Subscribers.Count(c => !c.IsDeleted);
            numberOfCopies = numberOfCopies <= 10 ? numberOfCopies : numberOfSubscribers / 10 * 10;
            var lastAddedBooks = _context.Books.Include(b => b.Author).Where(b => !b.IsDeleted).OrderByDescending(b => b.Id).Take(8).ToList();

            var topBooks = _context.RentalCopies
                .Include(c => c.BookCopy)
                .ThenInclude(c => c!.Book)
                .ThenInclude(b => b!.Author)
                .GroupBy(c => new
                {
                    c.BookCopy!.BookId,
                    c.BookCopy!.Book!.Title,
                    c.BookCopy.Book.ImageThumbnailUrl,
                    AuthorName = c.BookCopy.Book.Author.Name
                })
                .Select(b => new
                {
                    b.Key.BookId,
                    b.Key.Title,
                    b.Key.ImageThumbnailUrl,
                    b.Key.AuthorName,
                    Count = b.Count()
                })
                .OrderByDescending(b => b.Count)
                .Take(6)
                .Select(b => new BookViewModel
                {
                    Id = b.BookId,
                    Title = b.Title,
                    ImageThumbnailUrl = b.ImageThumbnailUrl,
                    Author = b.AuthorName

                }).ToList();
            DashboardViewModel viewModel = new DashboardViewModel()
            {
                NumberOfCopies = numberOfCopies,
                NumberOfSubscribers = numberOfSubscribers,
                LastAddedBooks = _mapper.Map<IEnumerable<BookViewModel>>(lastAddedBooks),
                TopBooks = topBooks
            };
            return View(viewModel);
        }

        [AjaxOnly]
        public IActionResult GetRentalsPerDay(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= DateTime.Today.AddDays(-29);
            endDate ??= DateTime.Today;

            var data = _context.RentalCopies
                .Where(c => c.RentalDate >= startDate && c.RentalDate <= endDate)
                .GroupBy(c => new { Date = c.RentalDate })
                .Select(g => new ChartItemViewModel
                {
                    Label = g.Key.Date.ToString("d MMM"),
                    Value = g.Count().ToString()

                }).ToList();
            List<ChartItemViewModel> figures = new List<ChartItemViewModel>();
            for (var day = startDate; day <= endDate; day = day.Value.AddDays(1))
            {

                var dayData = data.SingleOrDefault(d => d.Label == day.Value.ToString("d MMM"));
                ChartItemViewModel item = new ChartItemViewModel
                {
                    Label = day.Value.ToString("d MMM"),
                    Value = dayData is null ? "0" : dayData.Value
                };
                figures.Add(item);
            }

            return Ok(figures);
        }

        [AjaxOnly]
        public IActionResult GetSubscribersPerCity()
        {
            var subscribersPerArea = _context.Subscribers
                .Where(s => !s.IsDeleted)
                .Include(s => s.Governorate)
                .GroupBy(s => new {s.Governorate!.Name }).Select(g => new ChartItemViewModel
                {
                    Label = g.Key.Name,
                    Value = g.Count().ToString()
                }).ToList();
            return Ok(subscribersPerArea);
        }
    }
}
