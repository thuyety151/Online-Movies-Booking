using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Theater
    {
        public Theater()
        {
            Screen = new HashSet<Screen>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Hotline { get; set; }

        public virtual ICollection<Screen> Screen { get; set; }
    }
}
