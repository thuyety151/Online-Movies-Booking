using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Seat
    {
        public Seat()
        {
            Bill = new HashSet<Bill>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string IdTypesOfSeat { get; set; }
        public string IdScreen { get; set; }

        public virtual Screen IdScreenNavigation { get; set; }
        public virtual TypesOfSeat IdTypesOfSeatNavigation { get; set; }
        public virtual ICollection<Bill> Bill { get; set; }
    }
}
