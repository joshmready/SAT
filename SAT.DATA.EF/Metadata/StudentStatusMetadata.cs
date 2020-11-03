using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAT.DATA.EF
{
    class StudentStatusMetadata
    {
        [Required]
        [Display (Name = "SSID")]
        public int SSID { get; set; }

        [Required]
        [Display (Name = "Student Status Name")]
        public string SSName { get; set; }

        [Required]
        [UIHint("MultilineText")]
        [Display (Name = "Student Service Description")]
        public string SSDescription { get; set; }
    }

    [MetadataType(typeof(StudentStatusMetadata))]
    public partial class StudentStatus
    { }
}
