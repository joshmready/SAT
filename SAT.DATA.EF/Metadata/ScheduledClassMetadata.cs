using System.ComponentModel.DataAnnotations;


namespace SAT.DATA.EF
{
    class ScheduledClassMetadata
    {
        [Required]
        [Display(Name = "Scheduled Class ID")]
        public int ScheduldedClassId { get; set; }

        [Required]
        [Display(Name = "Course ID")]
        public int CourseId { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public System.DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public System.DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Instructor Name")]
        public string InstructorName { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "SCSID")]
        public int SCSID { get; set; }
    }
    [MetadataType(typeof(ScheduledClassMetadata))]
    public partial class ScheduledClass
    {
        [Display(Name = "Course Information")]
        public string CourseInfo
        {
            get { return StartDate + " " + Course.CourseName + " " + Location; }
        }
    }
}

