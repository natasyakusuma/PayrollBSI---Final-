using PayrollBSI.MVCProject.Services.Interface;

namespace PayrollBSI.MVCProject.Models
{
	public class AttendanceModel
	{
		public int AttendanceId { get; set; }
		public int EmployeeId { get; set; }
		public int OvertimeHours { get; set; }
		public int RegularHours { get; set; }
		public int AttendanceTotal { get; set; }
		public EmployeeModel Employee { get; set; }
	}
}
