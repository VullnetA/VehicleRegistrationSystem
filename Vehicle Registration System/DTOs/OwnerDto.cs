using Vehicle_Registration_System.Enums;

namespace Vehicle_Registration_System.DTOs
{
    public class OwnerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public DateTime LicenseIssueDate { get; set; }
    }
}
