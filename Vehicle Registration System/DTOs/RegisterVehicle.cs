using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vehicle_Registration_System.Enums;

namespace Vehicle_Registration_System.DTOs
{
    public class RegisterVehicle
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public Category Category { get; set; }
        public Transmission Transmission { get; set; }
        public Fuel Fuel { get; set; }
        public string LicensePlate { get; set; }
        public int OwnerId { get; set; }
        public int Power { get; set; }
    }
}
