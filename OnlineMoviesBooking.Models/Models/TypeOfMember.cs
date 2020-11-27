using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class TypeOfMember
    {
        public TypeOfMember()
        {
            Account = new HashSet<Account>();
        }

        public string IdTypeMember { get; set; }
        public string TypeOfMemberName { get; set; }
        public string Content { get; set; }
        public int? Point { get; set; }

        public virtual ICollection<Account> Account { get; set; }
    }
}
