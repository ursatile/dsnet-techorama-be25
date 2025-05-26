using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.PricingClient;

public class AutobarnPricingClientService(
	ILogger<AutobarnPricingClientService> logger,
	IBus bus
) : IHostedService {
	private const string SUBSCRIBER_ID = "autobarn.pricingclient";

	public async Task StartAsync(CancellationToken token) {
		logger.LogInformation("Starting the pricing client!");
		await bus.PubSub.SubscribeAsync<NewVehicleMessage>(
			SUBSCRIBER_ID, HandleNewVehicleMessage, token);
	}

	private Task HandleNewVehicleMessage(NewVehicleMessage message) {
		logger.LogInformation("New Vehicle Message: {message}", message);
		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Stopping the pricing client!");
		return Task.CompletedTask;
	}
}