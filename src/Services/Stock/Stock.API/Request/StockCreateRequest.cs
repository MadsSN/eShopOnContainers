using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.API.Request
{
    public class StockCreateRequest
    {
        [Range(0,1000000000)]
        public int StockTraderId { get; set; }

        [Range(0,10000000)]
        public int Shares { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }
    }
}
