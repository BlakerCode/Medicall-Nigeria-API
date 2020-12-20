using System;
using System.Collections.Generic;

namespace SpotOnAccountServer.Models
{
    public partial class Users
    {
        public Users()
        {
            Specialists = new HashSet<Specialists>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string StateOrigin { get; set; }
        public string LocalGovt { get; set; }
        public string DateRegistered { get; set; }
        public string SubscriptionDate { get; set; }
        public string SubscriptionExpires { get; set; }
        public bool? Busy { get; set; }
        public bool? IsDoc { get; set; }
        public string ConfirmationCode { get; set; }

        public ICollection<Specialists> Specialists { get; set; }
    }
}
