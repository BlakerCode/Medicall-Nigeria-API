using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotOnAccountServer.Dtos
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string LoginName { get; set; }
        public string StateOrigin { get; set; }
        public string LocalGovt { get; set; }
        public string DateRegistered { get; set; }
        public bool Expired { get; set; }
        public bool IsDoc { get; set; }
    }
}
