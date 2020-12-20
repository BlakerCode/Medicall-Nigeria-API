using System;
using System.Collections.Generic;

namespace SpotOnAccountServer.Models
{
    public partial class Treatments
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string DocId { get; set; }
        public string TreatmentFor { get; set; }
        public string Application { get; set; }
        public string DateTreated { get; set; }
        public bool? Viewed { get; set; }
        public string Symptom { get; set; }
    }
}
