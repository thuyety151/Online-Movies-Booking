using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Seat
    {
        public Seat()
        {
            Ticket = new HashSet<Ticket>();
        }

        public string Id { get; set; }
        public string IdTypesOfSeat { get; set; }
        public string IdScreen { get; set; }
        public string Row { get; set; }
        public int No { get; set; }

        public virtual Screen IdScreenNavigation { get; set; }
        public virtual TypesOfSeat IdTypesOfSeatNavigation { get; set; }
        public virtual ICollection<Ticket> Ticket { get; set; }
    }
}
