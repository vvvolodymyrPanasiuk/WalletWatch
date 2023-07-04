using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace WalletWatch.WebAPI.HealthChecks
{
    public class ApiHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ApiHealthCheck(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<HealthCheckResult> CheckHealthAsync
        (HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            //TODO: fix this request

            /*using (var httpClient = _httpClientFactory.CreateClient())
            {
                var obj = new Models.AuthenticationModels.Request.LoginRequest()
                {
                    Email = "fofofo@gmail.com",
                    Password = "Jfkfn645485"
                };
                
                string json = JsonConvert.SerializeObject(obj);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://localhost:5001/api/v1/Authentication/login", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    return await Task.FromResult(new HealthCheckResult(
                      status: HealthStatus.Healthy,
                      description: "The API is up and running."));
                }

                return await Task.FromResult(new HealthCheckResult(
                  status: HealthStatus.Unhealthy,
                  description: "The API is down."));
            }*/
            return await Task.FromResult(new HealthCheckResult(
                  status: HealthStatus.Unhealthy,
                  description: "The API is down."));
        }
    }
}
