using Newtonsoft.Json;
using StatisticalData.Infrastructure.Dtos;

namespace StatisticalData.Infrastructure
{
    public sealed class StatisticalDataAccessor : IStatisticalDataAccessor
    {
        private List<AreaItem> _areaItems = new List<AreaItem>();

        public async Task<List<AreaItem>> GetAll()
        {
            if(_areaItems.Any())
                return _areaItems;

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://api-dbw.stat.gov.pl/api/1.1.0/area/area-area");
                var stringContent = await response.Content.ReadAsStringAsync();
                var deserializedItems = JsonConvert.DeserializeObject<List<AreaItem>>(stringContent);

                if (deserializedItems is not null)
                    _areaItems = deserializedItems;
            }

            return _areaItems.ToList();
        }
    }
}
