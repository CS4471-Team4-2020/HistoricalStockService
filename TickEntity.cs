using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace HistoricalStockService
{
    class TickEntity : TableEntity
    {
        public TickEntity()
        {
        }
        public TickEntity(string company)
        {
            PartitionKey = company;
            RowKey = Guid.NewGuid().ToString("N");
            CompanyAbbrev = company;
        }
        public TickEntity(string companyAbbrev,string rowkey,DateTime date, string company, double price,int size, string exchange, string exchangeSymbol,string salecondition,bool suspicious)
        {
            PartitionKey = companyAbbrev;
            RowKey = Guid.NewGuid().ToString("N");
            Date = date;
            CompanyAbbrev = companyAbbrev;
            Company = company;
            Price = price;
            Size = size;
            Exchange = exchange;
            ExchangeSymbol = exchangeSymbol;
            SaleConditions = salecondition;
            IsSuspicious = suspicious;
        }
        public DateTime Date { get; set; }
        public string CompanyAbbrev { get; set; }
        public string Company { get; set; }
        public double Price { get; set; }
        public double RealPrice { get; set; }
        public int Size { get; set; }
        public string Exchange { get; set; }
        public string ExchangeSymbol { get; set; }
        public string SaleConditions { get; set; }
        public bool IsSuspicious { get; set; }
        public bool Closing { get; set; }
    }
}
