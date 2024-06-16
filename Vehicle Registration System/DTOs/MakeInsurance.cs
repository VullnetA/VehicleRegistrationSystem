using System.ComponentModel.DataAnnotations.Schema;
using Vehicle_Registration_System.Enums;

namespace Vehicle_Registration_System.DTOs
{
    public class MakeInsurance
    {
        public InsuranceCompany InsuranceCompany { get; set; }
        public int VehicleId { get; set; }
    }
}
