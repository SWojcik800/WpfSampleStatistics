﻿using StatisticalData.Infrastructure.Dtos;

namespace StatisticalData.Infrastructure
{
    public interface IStatisticalDataAccessor
    {
        void Create(AreaItem item);
        Task Delete(List<long> ids);
        Task<List<AreaItem>> GetAll();
        long GetMaxId();
        long ItemsCount();
        Task Update(AreaItem itemToUpdate);
    }
}