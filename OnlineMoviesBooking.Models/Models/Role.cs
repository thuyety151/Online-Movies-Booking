using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Role
    {
        public Role()
        {
            RoleClaim = new HashSet<RoleClaim>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<RoleClaim> RoleClaim { get; set; }
    }
}
