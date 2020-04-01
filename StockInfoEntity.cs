using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace HistoricalStockService
{
    class StockInfoEntity : TableEntity
    {
        public StockInfoEntity(string partition, string companySymbol, DateTime date, double price)
        {
            PartitionKey = partition;
            RowKey = companySymbol;
            Date = date;
            Price = price;
        }
        public StockInfoEntity() { }
        public DateTime Date { get; set; }
        public double Price { get; set; }
    }
}

