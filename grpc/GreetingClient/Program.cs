using GreetingServer;
using Grpc.Net.Client;

var names = new Stack<string>(["Alice", "Bryan", "Carol", "David", "Edgar", "Filip"]);

using var channel = GrpcChannel.ForAddress("http://localhost:5131");
var client = new Greeter.GreeterClient(channel);
while (true) {
	Console.WriteLine("Ready! Press a key to greet somebody!");
	Console.ReadKey();
	var req = new HelloRequest {
		Name = names.Pop()
	};
	var reply = await client.SayHelloAsync(req);
	Console.WriteLine(reply.Message);
}

