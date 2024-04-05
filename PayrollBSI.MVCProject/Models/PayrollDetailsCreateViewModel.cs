using Microsoft.AspNetCore.Mvc.Rendering;

namespace PayrollBSI.MVCProject.Models
{
	public class PayrollDetailsCreateViewModel
	{
		public IEnumerable<SelectListItem> Employee { get; set; }
		// Other properties as needed

		public IEnumerable<SelectListItem> Attendance { get; set; }

		public PayrollDetailsModel PayrollDetail { get; set; }

	}
}
