using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Discount
    {
        public Discount()
        {
            Ticket = new HashSet<Ticket>();
            UseDiscount = new HashSet<UseDiscount>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? PercentDiscount { get; set; }
        public int? MaxCost { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string ImageDiscount { get; set; }
        public int? Point { get; set; }
        public int? Used { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Ticket> Ticket { get; set; }
        public virtual ICollection<UseDiscount> UseDiscount { get; set; }
    }
}
