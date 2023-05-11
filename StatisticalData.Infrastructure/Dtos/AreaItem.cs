using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticalData.Infrastructure.Dtos
{
    public class AreaItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ParentId { get; set; }
        public long LevelId { get; set; }
        public string LevelName { get; set; }
        public bool CanChange { get; set; }
    }
}
