using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace user_interface
{
    public class Settings
    {
        public string BaseUri { get; set; }
    }

    public class InsuranceProduct
    {
        public int id { get; set; }
        public string provider { get; set; }
        public string type { get; set; }

        private decimal _maxSumInsurable;
        public decimal maxSumInsurable { get => _maxSumInsurable; set => _maxSumInsurable = Math.Round(value, 2); }

        private decimal _monthlyPremium;
        public decimal monthlyPremium { get => _monthlyPremium; set => _monthlyPremium = Math.Round(value, 2); }

        private decimal _gst;
        public decimal gst { get => _gst; set => _gst = Math.Round(value, 2); }

        public decimal total => monthlyPremium + gst;
    }


}

namespace user_interface.Pages
{

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public HttpClient Client { get; }

        public Settings Settings { get; }

        public IEnumerable<InsuranceProduct> Products { get; set; } = new List<InsuranceProduct>();

        public IndexModel(ILogger<IndexModel> logger, IOptions<Settings> settings, IHttpClientFactory clientFactory)
        {
            _logger = logger;

            Client = clientFactory.CreateClient();
            Settings = settings.Value;
        }

        public async Task OnGet()
        {
            try
            {
                var response = await Client.GetAsync(Settings.BaseUri + "/products");

                if (response.IsSuccessStatusCode)
                {
                    Products = await JsonSerializer.DeserializeAsync
                                    <IEnumerable<InsuranceProduct>>
                                    (await response.Content.ReadAsStreamAsync());
                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();
            }
        }
    }
}
