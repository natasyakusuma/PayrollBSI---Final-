using PayrollBSI.MVCProject.Models;


namespace PayrollBSI.MVCProject.Services.Interface
{
    public interface IPosition
    {
        Task<PositionModel> CreatePosition(PositionModel position);
        Task<PositionModel> UpdatePosition(int id, PositionModel position);
        Task<PositionModel> GetPositionById(int id);
        Task<IEnumerable<PositionModel>> GetAllPositions();
        Task<bool> DeletePosition(int id);
        Task<IEnumerable<PositionModel>> GetAllActivePositions();
    }
}
