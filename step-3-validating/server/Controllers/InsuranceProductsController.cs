using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace server.Controllers
{
    public class InsuranceProduct
    {
        public int id { get; set; }
        public string provider { get; set; }
        public string type { get; set; }

        private decimal _maxSumInsurable;
        public decimal maxSumInsurable { get => _maxSumInsurable; set => _maxSumInsurable = Math.Round(value, 2); }

        private decimal _monthlyPremium;
        public decimal monthlyPremium { get => _monthlyPremium; set => _monthlyPremium = Math.Round(value, 2); }

        //private decimal _gst;
        //public decimal gst { get => _gst; set => _gst = Math.Round(value, 2); }

        //trying to cause a failure with a bad type
        public string gst { get; set; }
    }

    [Route("/products")]
    [ApiController]
    public class InsuranceProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(SimulatingACallToADataSource());
        }

        IEnumerable<InsuranceProduct> SimulatingACallToADataSource()
        {
            List<InsuranceProduct> someProducts = new List<InsuranceProduct>();
            //Simulating a call to a data store to get information
            for(int i=1; i<=20; i++)
            {
                someProducts.Add(new InsuranceProduct
                {
                    id = i,
                    provider = i % 2 == 0 ? "GIO" : "AIA",
                    type = i % 2 == 0 ? "Comprehensive Car" : "Third Party Car",
                    maxSumInsurable = i % 2 == 0 ? 50_000 : 5_000,
                    monthlyPremium = i % 2 == 0 ? 800 : 200,
                    gst = i % 2 == 0 ? "10%" : "20%"
                });
            }

            return someProducts;
        }
    }
}
