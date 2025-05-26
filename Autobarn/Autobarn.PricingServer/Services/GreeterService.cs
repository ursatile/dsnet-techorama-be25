using Autobarn.Pricing;
using Grpc.Core;

namespace Autobarn.PricingServer.Services;

public class PricerService(
	ILogger<PricerService> logger) : Pricer.PricerBase {
    public override Task<PriceReply> GetPrice(PriceRequest request, ServerCallContext context) {
	    return Task.FromResult(new PriceReply {
		    Currency = "EUR",
		    Price = 75000
	    });
    }
}
