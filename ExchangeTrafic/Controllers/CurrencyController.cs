using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Data.SqlClient;

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
            }
            else
            {
                return "NotFound";
            }

            return SaveInputs;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("apikey", "vbjeljWWPNFs5FGuVoXeefSI6jdplMS1");

            string url = @"https://api.apilayer.com/exchangerates_data/latest";
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseJson = JsonSerializer.Deserialize<Dictionary<string, object>>(responseString);

                // Создаем соединение с базой данных
                string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=VegaVestaNew;Integrated Security=True";
                SqlConnection connection = new SqlConnection(connectionString);

                connection.Open();

                // Создаем SQL-запрос INSERT для добавления данных в таблицу
                string insertQuery = "INSERT INTO Options ([URL], Headers, Setting1, Setting2, Setting3) VALUES (@url, @headers, @setting1, @setting2, @setting3)";
                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@url", url);
                command.Parameters.AddWithValue("@headers", JsonSerializer.Serialize(httpClient.DefaultRequestHeaders));
                command.Parameters.AddWithValue("@setting1", JsonSerializer.Serialize(responseJson["setting1"]));
                command.Parameters.AddWithValue("@setting2", JsonSerializer.Serialize(responseJson["setting2"]));
                command.Parameters.AddWithValue("@setting3", JsonSerializer.Serialize(responseJson["setting3"]));

                // Выполняем SQL-запрос INSERT
                command.ExecuteNonQuery();

                // Закрываем соединение с базой данных
                connection.Close();

                return Ok(responseJson);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
