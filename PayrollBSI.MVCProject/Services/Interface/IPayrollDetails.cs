using PayrollBSI.MVCProject.Models;

namespace PayrollBSI.MVCProject.Services.Interface
{
	public interface IPayrollDetails
	{
        Task<IEnumerable<PayrollDetailsModel>> GetAll();
        Task<IEnumerable<AttendanceModel>> GetAllAttendances();
        Task<PayrollDetailsModel> GetById(int id);
        Task<bool> Insert( int employeeId, int attendanceId);

    }
}
