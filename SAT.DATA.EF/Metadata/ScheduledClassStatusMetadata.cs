using System.ComponentModel.DataAnnotations;

namespace SAT.DATA.EF
{
    class ScheduledClassStatusMetadata
    {
        [Display(Name = "Status")]
        public string SCSName { get; set; }
    }

    [MetadataType(typeof(ScheduledClassStatusMetadata))]
    public partial class ScheduledClassStatus
    { }
}
