namespace PayrollBSI.MVCProject.Models
{
    public class PositionModel
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; } 
        public decimal AllowanceMeal { get; set; }
        public decimal AllowanceTransport { get; set; }
        public decimal DeductionPension { get; set; }
        public decimal DeductionInsurance { get; set; }
        public decimal PayrateOvertime { get; set; }
        public decimal PayrateRegular { get; set; }

        public bool? IsDeleted { get; set; } // Change from int to bool?
        public bool? IsActive { get; set; } // Change from int to bool?
    }
}
