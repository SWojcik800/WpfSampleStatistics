using StatisticalData.Infrastructure.Dtos;

namespace StatisticalData.Infrastructure
{
    public interface IStatisticalDataAccessor
    {
        Task Create(AreaItem item);
        Task Delete(List<long> ids);
        Task<List<AreaItem>> GetAll();
        long ItemsCount();
        Task Update(AreaItem itemToUpdate);
    }
}