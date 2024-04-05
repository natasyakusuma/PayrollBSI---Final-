namespace PayrollBSI.MVCProject.Models
{
	public class PayrollDetailsModel
	{
		public int PayrollDetailId { get; set; }
		public int EmployeeId { get; set; }
		public DateOnly PayrollDate { get; set; }
		public decimal TotalAllowances { get; set; }
		public decimal TotalDeductions { get; set; }

		public decimal GrossPay { get; set; }

		public decimal NetPayNoTax { get; set; }

		public decimal NetPayWithTax { get; set; }

		public virtual EmployeeModel Employee { get; set; } = null!;
	}
}
