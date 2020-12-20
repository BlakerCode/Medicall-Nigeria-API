using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotOnAccountServer.Dtos
{
    public class DocUserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Language { get; set; }
        public string Photo { get; set; }
        public bool? Busy { get; set; }
        public string Special { get; set; }
        public string LoginName { get; set; }
    }
}
