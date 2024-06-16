using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using Vehicle_Registration_System.Enums;

namespace Vehicle_Registration_System.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public Category Category { get; set; }
        public Transmission Transmission { get; set; }
        public int Power { get; set; }
        public Fuel Fuel { get; set; }
        public string LicensePlate { get; set; }
        public DateTime DateRegistered { get; set; }
        public DateTime ExpirationDate { get; set; }
        [ForeignKey("Owner")]
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
        public Insurance Insurance { get; set; }
    }
}
