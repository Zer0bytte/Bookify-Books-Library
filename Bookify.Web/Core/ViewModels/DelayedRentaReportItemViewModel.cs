namespace Bookify.Web.Core.ViewModels
{
    public class DelayedRentaReportItemViewModel
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; } = null!;
        public string SubscriberPhone { get; set; }=null!;
        public string BookTitle { get; set; } = null!;
        public int BookSerial { get; set; }
        public string RentalDate { get; set; } = null!;
        public DateTime EndDate { get; set; } 
        public string? ExtendedOn { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int DelayInDays
        {
            get
            {
                var delay = 0;

                if (ReturnDate.HasValue && ReturnDate.Value > EndDate)
                    delay = (int)(ReturnDate.Value - EndDate).TotalDays;

                else if (!ReturnDate.HasValue && DateTime.Today > EndDate)
                    delay = (int)(DateTime.Today - EndDate).TotalDays;

                return delay;
            }
        }
    }
}
