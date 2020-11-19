using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class RoleClaim
    {
        public string IdTypesOfAccount { get; set; }
        public string IdRole { get; set; }
        public string Name { get; set; }

        public virtual Role IdRoleNavigation { get; set; }
        public virtual TypesOfAccount IdTypesOfAccountNavigation { get; set; }
    }
}
