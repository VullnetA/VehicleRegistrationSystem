using System.ComponentModel.DataAnnotations;
using Vehicle_Registration_System.Enums;

namespace Vehicle_Registration_System.Models
{
    public class Owner
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public DateTime LicenseIssueDate { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
