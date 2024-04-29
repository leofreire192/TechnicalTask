using CustomersAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PinewoodTechnicalTask.Pages
{
    public class ViewCustomersModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ViewCustomersModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public List<Customer> Customers { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            using (var client = _httpClientFactory.CreateClient("MyApiClient"))
            {
                var response = await client.GetAsync("customer");
                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Failed to retrieve customers.");
                    return Page();
                }

                var content = await response.Content.ReadAsStringAsync();
                Customers = JsonConvert.DeserializeObject<List<Customer>>(content);
            }

            return Page();
        }

        public async Task<IActionResult> PatchCustomerAsync(Guid customerId, Customer customer)
        {
            using (var client = _httpClientFactory.CreateClient("MyApiClient"))
            {
                var url = $"customer/{customerId}";
                var patchContent = new StringContent(JsonConvert.SerializeObject(customer), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PatchAsync(url, patchContent);

                if (response.IsSuccessStatusCode)
                {
                    // Optionally handle success
                    return Page();
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update customer.");
                    return Page();
                }
            }
        }
    }
}
