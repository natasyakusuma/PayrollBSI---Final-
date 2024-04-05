using PayrollBSI.MVCProject.Models;

namespace PayrollBSI.MVCProject.Services.Interface
{
	public interface IEmployee
	{
		Task<string> ChangePassword(string username, string newPassword);
		Task<IEnumerable<EmployeeModel>> GetAll();
		Task<EmployeeModel> GetById(int id);
		Task<LoginResponseModel> Login(string username, string password);
		Task<IEnumerable<EmployeeModel>> GetAllActive();
		Task<IEnumerable<EmployeeModel>> Search(string employeeName, string positionName, string roleName);

		Task<string> GetByUsername(string username);
	}
}
