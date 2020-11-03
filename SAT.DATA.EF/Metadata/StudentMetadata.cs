using System.ComponentModel.DataAnnotations;

namespace SAT.DATA.EF
{
    class StudentMetadata
    {
        
        [Required]
        [Display(Name = "First Name")]
        [DisplayFormat(NullDisplayText = "*First Name is required")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [DisplayFormat(NullDisplayText = "*Last Name is required")]
        public string LastName { get; set; }

        [Required]
        [DisplayFormat(NullDisplayText = "*Major is required. If student is undecided, set to Undeclared")]
        public string Major { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required]
        [DisplayFormat(NullDisplayText = "*Email is required")]
        public string Email { get; set; }
    }
    
    [MetadataType(typeof(StudentMetadata))]
    public partial class Student
    {
       public string Fullname
       {
            get { return FirstName + " " + LastName; }
       }
    }
}
