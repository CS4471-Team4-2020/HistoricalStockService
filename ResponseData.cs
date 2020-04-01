using System;
using System.Collections.Generic;
using System.Text;

namespace HistoricalStockService
{
    class ResponseData
    {
        public ResponseData()
        {
            Dates = new List<string>();
            Prices = new List<double>();
        }
        public List<string> Dates { get; set; }
        public List<double> Prices { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
    }
}
