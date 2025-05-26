using Grpc.Core;
using GreetingServer;

namespace GreetingServer.Services;

public class GreeterService : Greeter.GreeterBase {
	private readonly ILogger<GreeterService> logger;
	public GreeterService(ILogger<GreeterService> logger) {
		this.logger = logger;
	}

	public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
		var message = request.LanguageCode switch {
			"be-NL" => $"Hallo {request.Name}",
			"be-FR" => $"Bonjour, {request.Name}",
			"be-DE" => $"Guten tag, {request.Name}",
			_ => $"Hi {request.Name}",
		};
		return Task.FromResult(new HelloReply {
			Message = message
		});
	}
}
