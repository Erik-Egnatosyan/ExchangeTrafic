using System.Data.SqlClient;//
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;//
using System.Net;
using System.Text.Json;
using Newtonsoft.Json.Linq;//
using Newtonsoft.Json;
using System.Configuration;
using System.Data;



namespace ExchangeTrafic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        [HttpGet("GetRates")]
        public async Task<string> GetRatesAsync()
        {
            string SaveInputs = string.Empty;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("apikey", "vbjeljWWPNFs5FGuVoXeefSI6jdplMS1");

            string url = @"https://api.apilayer.com/exchangerates_data/latest";
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                SaveInputs = await response.Content.ReadAsStringAsync();
                Console.WriteLine(SaveInputs);
            }
            else
            {
                return "NotFound";
            }
            return SaveInputs;
        }
        [HttpGet("GetJsonRates")]
        public async Task<IActionResult> GetJsonRatesAsync()
        {
            string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Rates;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string SaveInputs = string.Empty;
            Dictionary<string, decimal> keyValuePairs = new Dictionary<string, decimal>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("apikey", "vbjeljWWPNFs5FGuVoXeefSI6jdplMS1");

            string url = @"https://api.apilayer.com/exchangerates_data/latest";
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                SaveInputs = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(SaveInputs);
                keyValuePairs = json["rates"].ToObject<Dictionary<string, decimal>>();

                string insertCommand = "INSERT INTO JsonKeyValues (Currency, RateMoney) VALUES (@currency, @rate)";
                SqlCommand command = new SqlCommand(insertCommand, connection);

                foreach (KeyValuePair<string, decimal> pair in keyValuePairs)
                {
                    command.Parameters.AddWithValue("@currency", pair.Key);
                    command.Parameters.AddWithValue("@rate", pair.Value);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }
            else
            {
                return NotFound();
            }

            connection.Close();
            return Ok();
        }

    }
}
