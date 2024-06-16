using System.ComponentModel.DataAnnotations.Schema;
using Vehicle_Registration_System.Enums;
using Vehicle_Registration_System.Models;

namespace Vehicle_Registration_System.DTOs
{
    public class InsuranceDto
    {
        public int Id { get; set; }
        public InsuranceCompany InsuranceCompany { get; set; }
        public float InsuranceFee { get; set; }
        public VehicleDto Vehicle { get; set; }
        public DateTime DateRegistered { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
