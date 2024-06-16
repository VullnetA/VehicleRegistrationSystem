using System.ComponentModel.DataAnnotations.Schema;
using Vehicle_Registration_System.Enums;

namespace Vehicle_Registration_System.Models
{
    public class Insurance
    {
        public int Id { get; set; }
        public InsuranceCompany InsuranceCompany { get; set; }
        public float InsuranceFee { get; set; }
        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime DateRegistered { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
