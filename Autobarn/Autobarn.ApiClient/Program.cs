using Autobarn.ApiClient;
using Microsoft.Extensions.Logging;

var http = new HttpClient() {
	BaseAddress = new Uri("https://autobarn.dev")
};
var logger = LoggerFactory.Create(lb
	=> lb.AddConsole()).CreateLogger<AutobarnApiClient>();
var client = new AutobarnApiClient(http, logger);
foreach (var model in await client.ListModelCodes()) {
	Console.WriteLine(model.Code);
}