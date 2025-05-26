using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.Notifier;

public class AutobarnNotifierService(
	ILogger<AutobarnNotifierService> logger,
	IBus bus
) : IHostedService {
	private const string SUBSCRIBER_ID = "autobarn.notifier";

	public async Task StartAsync(CancellationToken token) {
		await bus.PubSub.SubscribeAsync<NewVehiclePriceMessage>(
			SUBSCRIBER_ID, HandleNewVehiclePriceMessage, token);
	}

	private void HandleNewVehiclePriceMessage(NewVehiclePriceMessage message) {
		logger.LogInformation("{message} {price} {currency}", message, message.Price, message.Currency);
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		return Task.CompletedTask;
	}
}