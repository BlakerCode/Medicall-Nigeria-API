using System;
using System.Collections.Generic;

namespace SpotOnAccountServer.Models
{
    public partial class CallLists
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? DocId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LoginName { get; set; }
    }
}
