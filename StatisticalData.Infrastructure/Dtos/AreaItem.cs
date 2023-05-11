using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticalData.Infrastructure.Dtos
{
    public class AreaItem
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("nazwa")]
        public string Name { get; set; }
        [JsonProperty("id-nadrzedny-element")]
        public long ParentId { get; set; }
        [JsonProperty("id-poziom")]
        public long LevelId { get; set; }
        [JsonProperty("nazwa-poziom")]
        public string LevelName { get; set; }
        [JsonProperty("czy-zmienne")]
        public bool CanChange { get; set; }
    }
}
