using System;
using System.Net.Http;
using System.Threading.Tasks;
using CustomersAPI.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PinewoodTechnicalTask.Client;

namespace PinewoodTechnicalTask.Pages
{
    public class CreateCustomerModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateCustomerModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        [BindProperty]
        public CustomerCreateRequest CustomerCreateRequest { get; set; }

        public string SuccessMessage { get; private set; }

        public string FailureReason { get; private set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var client = _httpClientFactory.CreateClient("MyApiClient"))
            {
                var response = await client.PostAsync("customer/create", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(CustomerCreateRequest), System.Text.Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    SuccessMessage = "Customer created successfully!";
                    return Page();
                }
                else
                {
                    FailureReason = "Failed to create customer. " + response.ReasonPhrase;
                    return Page();
                }
            }
        }
    }
}
