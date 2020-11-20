using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class TypesOfSeat
    {
        public TypesOfSeat()
        {
            Seat = new HashSet<Seat>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }

        public virtual ICollection<Seat> Seat { get; set; }
    }
}
