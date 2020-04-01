using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions;
using System.Net.Http;
using System;
using System.Linq;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HistoricalStockService
{
    public static class Function1
    {
        [FunctionName("ProcessData")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            RequestData data = JsonConvert.DeserializeObject<RequestData>(content);
            string company = data.Company;
            DateTime startDate = data.StartDate;
            DateTime endDate = data.EndDate;
            var client = GetCloudClient();
            var tickerTable = client.GetTableReference("tickerdata");
            var table = client.GetTableReference("stocksinfo");
            var aggTable = client.GetTableReference("groupedtickerdata");

            ResponseData newResponse = new ResponseData();
            List<TickEntity> aggtickerList;
            //For one day time range
            if (startDate.Date == endDate.Date)
            {
                aggtickerList = aggTable.CreateQuery<TickEntity>()
                .Where(x => x.PartitionKey == company && x.Date >= startDate && x.Date <= endDate)
                .Select(x => new TickEntity() { PartitionKey = x.PartitionKey, RowKey = x.RowKey, Date = x.Date, Price = x.Price })
                .ToList()
                .OrderBy(x => x.Date)
                .ToList();
                foreach (var tick in aggtickerList)
                {
                    //Use times instead of dates
                    newResponse.Dates.Add(tick.Date.ToString("HH:mm"));
                    newResponse.Prices.Add(tick.Price);
                }
            }
            else
            {
                //Get ticker data if it is a closing price
                aggtickerList = aggTable.CreateQuery<TickEntity>()
                .Where(x => x.PartitionKey == company && x.Date >= startDate && x.Date <= endDate && x.Closing)
                .Select(x => new TickEntity() { PartitionKey = x.PartitionKey, RowKey = x.RowKey, Date = x.Date, Price = x.Price })
                .ToList()
                .OrderBy(x => x.Date)
                .ToList();
                foreach(var tick in aggtickerList)
                {
                    newResponse.Dates.Add(tick.Date.ToString("MMM d"));
                    newResponse.Prices.Add(tick.Price);
                }
            }

            return new OkObjectResult(newResponse);

        }


        public static CloudTableClient GetCloudClient()
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=storageaccount4471b6a6;AccountKey=dX2VUCuxC0EcRnyZ7Srg+XIKLLagAO30kkpcBcsv9bq91rG+h2FomX6EHP/IByNzKSxVdRIqq6phUYDQ3PAPnw==;EndpointSuffix=core.windows.net";
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            return tableClient;
        }
    }
}
