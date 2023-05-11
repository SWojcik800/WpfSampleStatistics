using StatisticalData.Infrastructure.Dtos;

namespace StatisticalData.Infrastructure
{
    public interface IStatisticalDataAccessor
    {
        Task<List<AreaItem>> GetAll();
        Task Update(AreaItem itemToUpdate);
    }
}