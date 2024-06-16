using System.ComponentModel.DataAnnotations.Schema;
using Vehicle_Registration_System.Enums;
using Vehicle_Registration_System.Models;

namespace Vehicle_Registration_System.DTOs
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public Category Category { get; set; }
        public string LicensePlate { get; set; }
        public DateTime DateRegistered { get; set; }
        public DateTime ExpirationDate { get; set; }
        public OwnerDto Owner { get; set; }
    }
}
