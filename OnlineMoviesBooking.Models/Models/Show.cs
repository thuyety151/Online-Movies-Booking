using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Show
    {
        public Show()
        {
            Bill = new HashSet<Bill>();
        }

        public string Id { get; set; }
        public string Languages { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string IdMovie { get; set; }
        public string IdScreen { get; set; }

        public virtual Movie IdMovieNavigation { get; set; }
        public virtual Screen IdScreenNavigation { get; set; }
        public virtual ICollection<Bill> Bill { get; set; }
    }
}
