using Autobarn.Messages;
using Autobarn.Pricing;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.PricingClient;

public class AutobarnPricingClientService(
	ILogger<AutobarnPricingClientService> logger,
	IBus bus,
	Pricer.PricerClient pricer
) : IHostedService {
	private const string SUBSCRIBER_ID = "autobarn.pricingclient";

	public async Task StartAsync(CancellationToken token) {
		logger.LogInformation("Starting the pricing client!");
		await bus.PubSub.SubscribeAsync<NewVehicleMessage>(
			SUBSCRIBER_ID, HandleNewVehicleMessage, token);
	}

	private async Task HandleNewVehicleMessage(NewVehicleMessage message) {
		logger.LogInformation("New Vehicle Message: {message}", message);
		var priceRequest = new PriceRequest {
			Year = message.Year,
			Color = message.Color,
			Make = message.Manufacturer,
			Model = message.ModelName
		};
		var price = await pricer.GetPriceAsync(priceRequest);
		logger.LogInformation("Got a price: {price} {currency}", price.Price, price.Currency);
		var newVehiclePriceMessage = message.WithPrice(price.Price, price.Currency);
		await bus.PubSub.PublishAsync(newVehiclePriceMessage);
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Stopping the pricing client!");
		return Task.CompletedTask;
	}
}