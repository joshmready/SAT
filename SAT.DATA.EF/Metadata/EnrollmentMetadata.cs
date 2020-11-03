using System.ComponentModel.DataAnnotations;

namespace SAT.DATA.EF
{
    class EnrollmentMetadata
    {
        [Required]
        [Display(Name = "Student ID")]
        [DisplayFormat(NullDisplayText = "*Please enter Student ID")]
        public int StudentId { get; set; }

        [Required]
        [Display(Name = "Class")]
        [DisplayFormat(NullDisplayText = "*Please enter class to enroll")]
        public int ScheduledClassId { get; set; }

        [Required]
        [Display(Name = "Enrollment Date")]
        [DisplayFormat(NullDisplayText = "*Please enter enrollment date", DataFormatString = "{0:d}")]
        public System.DateTime EnrollmentDate { get; set; }
    }

    [MetadataType(typeof(EnrollmentMetadata))]
    public partial class Enrollment
    {}
}
