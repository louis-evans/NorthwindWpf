using Newtonsoft.Json;
using NorthwindWpf.Data.Models;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace NorthwindWpf.Data.Services
{
    public class AddressLookupService : IDisposable
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public AddressLookupService()
        {
            _apiKey = ConfigurationManager.AppSettings["AddressApiKey"];
            _httpClient = new HttpClient();
        }

        public async Task<AddressFindResult> FindByPostCodeAsync(string postCode, int? number = null)
        {
            if (string.IsNullOrWhiteSpace(postCode)) throw new ArgumentNullException(nameof(postCode));

            string jsonText;

#if DEBUG
            var fileName = number == null ? "find_address_by_postcode" : "find_address_by_number";

            var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Static", $"{fileName}_sample.json");
            jsonText = File.ReadAllText(filePath);
            
#else
            var apiUrl = ConfigurationManager.AppSettings["AddressFindUrl"];
            var urlEnd = postCode.Trim().Replace(" ", "") + (number == null ? "" : $"/{number}");

            apiUrl = string.Format(apiUrl, urlEnd, _apiKey);

            var response = await _httpClient.GetAsync(apiUrl);
            jsonText = await response.Content.ReadAsStringAsync();
#endif

            return JsonConvert.DeserializeObject<AddressFindResult>(jsonText);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}

