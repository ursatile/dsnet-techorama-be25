using Autobarn.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Autobarn.Notifier;

public class AutobarnNotifierService(
	ILogger<AutobarnNotifierService> logger,
	IBus bus,
	HubConnection hub
) : IHostedService {
	private const string SUBSCRIBER_ID = "autobarn.notifier";

	public async Task StartAsync(CancellationToken token) {
		await hub.StartAsync(token);
		await bus.PubSub.SubscribeAsync<NewVehiclePriceMessage>(
			SUBSCRIBER_ID, HandleNewVehiclePriceMessage, token);
	}

	private async Task HandleNewVehiclePriceMessage(NewVehiclePriceMessage message) {
		logger.LogInformation("{message} {price} {currency}", message, message.Price, message.Currency);
		var json = JsonConvert.SerializeObject(message);
		await hub.SendAsync("NotifyWebsiteUsers", "autobarn.notifier", json);
		logger.LogInformation("Price sent to SignalR Hub");
	}

	public async Task StopAsync(CancellationToken token) {
		await hub.StopAsync(token);
	}
}