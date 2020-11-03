using System.ComponentModel.DataAnnotations;

namespace SAT.DATA.EF
{
    class CourseMetadata
    {
        [Required]
        [Display(Name = "Course Name")]
        [DisplayFormat(NullDisplayText = "*Please enter Course Name")]
        public string CourseName { get; set; }

        [Display(Name = "Course Description")]
        [UIHint("MultilineText")]
        public string CourseDescription { get; set; }

        [Required]
        [Display(Name = "Credit Hours")]
        [DisplayFormat(NullDisplayText = "*Please enter Credit Hours")]
        public byte CreditHours { get; set; }

        [UIHint("MultilineText")]
        public string Notes { get; set; }

    }
    [MetadataType(typeof(CourseMetadata))]
    public partial class Course
    { }
}
