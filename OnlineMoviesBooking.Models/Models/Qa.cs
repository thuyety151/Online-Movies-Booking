using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Qa
    {
        public string Id { get; set; }
        public string IdAccount { get; set; }
        public string Email { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }

        public virtual Account IdAccountNavigation { get; set; }
    }
}
