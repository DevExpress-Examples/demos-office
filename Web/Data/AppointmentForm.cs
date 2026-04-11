using System.ComponentModel.DataAnnotations;

namespace BlazorDemo.OfficeFileApi
{
    public class AppointmentForm
    {
        [StringLength(100, ErrorMessage = "Patient name cannot exceed 100 characters")]
        public string PatientName { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; } 

        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(100, ErrorMessage = "Email address cannot exceed 100 characters")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; }

        [StringLength(20, ErrorMessage = "Appointment time cannot exceed 20 characters")]
        [Display(Name = "Appointment Time")]
        public string AppointmentTime { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Doctor's name cannot exceed 100 characters")]
        [Display(Name = "Doctor's Name")]
        public string DoctorName { get; set; } = string.Empty;

        [Display(Name = "Visit Reason")]
        public VisitReason VisitReason { get; set; }

        [StringLength(500, ErrorMessage = "Additional notes cannot exceed 500 characters")]
        [Display(Name = "Additional Notes")]
        public string AdditionalNotes { get; set; } = string.Empty;

        [Display(Name = "Wheelchair Access Required")]
        public bool WheelchairAccess { get; set; }

        [Display(Name = "Language Interpreter Required")]
        public bool LanguageInterpreter { get; set; }

        [Display(Name = "Other Requirements")]
        public bool OtherRequirements { get; set; }

        [StringLength(200, ErrorMessage = "Other requirement description cannot exceed 200 characters")]
        [Display(Name = "Other Requirement Details")]
        public string OtherRequirement { get; set; } = string.Empty;

    }
    public enum VisitReason
    {
        [Display(Name = "Annual Physical Examination")]
        AnnualPhysicalExamination,

        [Display(Name = "Follow-up Visit")]
        FollowUpVisit,

        [Display(Name = "Specific Concern")]
        SpecificConcern
    }
}