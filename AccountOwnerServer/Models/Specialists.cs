using System;
using System.Collections.Generic;

namespace SpotOnAccountServer.Models
{
    public partial class Specialists
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Specialization { get; set; }
        public string Language { get; set; }
        public string Photo { get; set; }

        public Users User { get; set; }
    }
}
