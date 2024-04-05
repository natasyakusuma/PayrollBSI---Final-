
using System.ComponentModel.DataAnnotations;

namespace PayrollBSI.MVCProject.Models
{
	public class EmployeeModel
	{
		public int EmployeeId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int RoleId { get; set; }
		public int PositionId { get; set; }

		[Required, StringLength(50)]
		public string Username { get; set; }

		[Required, StringLength(50)]
		public string Password { get; set; }
		public int IsDeleted { get; set; }
		public RoleModel Role { get; set; }
		public PositionModel Position { get; set; }
	}
}
