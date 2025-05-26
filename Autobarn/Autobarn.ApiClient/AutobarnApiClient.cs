using System.Text.Json;
using Autobarn.Data.Entities;
using Microsoft.Extensions.Logging;

namespace Autobarn.ApiClient;

public class AutobarnApiClient(HttpClient http, ILogger<AutobarnApiClient> logger) {

	private JsonSerializerOptions options = new(JsonSerializerDefaults.Web);

	public async Task<CarModel[]> ListModelCodes() {
		var json = await http.GetStringAsync("/api/models");
		logger.LogInformation(json);
		var models = JsonSerializer.Deserialize<CarModel[]>(json, options)!;
		return models;
	}
}