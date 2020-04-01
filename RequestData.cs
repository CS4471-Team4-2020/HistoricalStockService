using System;
using System.Collections.Generic;
using System.Text;

namespace HistoricalStockService
{
    class RequestData
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Company { get; set; }
    }
}
