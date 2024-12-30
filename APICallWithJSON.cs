using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiService> _logger;

    public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task CallPythonApiAsync(string jsonFilePath, string apiEndpoint)
    {
        try
        {
            var jsonString = await File.ReadAllTextAsync(jsonFilePath);

            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiEndpoint, content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully called the Python API.");
            }
            else
            {
                _logger.LogError($"Failed to call the Python API. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while calling the Python API: {ex.Message}");
        }
    }
}
