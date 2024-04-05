using PayrollBSI.MVCProject.Models;

namespace PayrollBSI.MVCProject.Services.Interface
{
	public interface IAttendance
	{
		Task<AttendanceModel> Insert(AttendanceModel attendance);
		Task<AttendanceModel> Update(int id, AttendanceModel obj);
		Task<AttendanceModel> GetById(int id);
		Task<IEnumerable<AttendanceModel>> GetAllAttendance();
		Task<IEnumerable<AttendanceModel>> GetByEmployeeId(int employeeId);

	}
}
