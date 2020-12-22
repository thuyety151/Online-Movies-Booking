using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Account
    {
        public Account()
        {
            Qa = new HashSet<Qa>();
            Ticket = new HashSet<Ticket>();
            UseDiscount = new HashSet<UseDiscount>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public string Sdt { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Point { get; set; }
        public string IdTypesOfUser { get; set; }
        public string IdTypeOfMember { get; set; }
        public string Image { get; set; }

        public virtual TypesOfAccount IdTypesOfUserNavigation { get; set; }
        public virtual ICollection<Qa> Qa { get; set; }
        public virtual ICollection<Ticket> Ticket { get; set; }
        public virtual ICollection<UseDiscount> UseDiscount { get; set; }
    }
}
