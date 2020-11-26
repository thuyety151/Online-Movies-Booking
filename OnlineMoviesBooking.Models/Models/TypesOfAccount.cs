using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class TypesOfAccount
    {
        public TypesOfAccount()
        {
            Account = new HashSet<Account>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Account> Account { get; set; }
    }
}
