using EasyNetQ;
using Messages;

Console.WriteLine("Welcome to the publisher!");
var amqp = "amqps://csrsgicq:7PAjB9tTm2zcajhlCPr4y3uxqtGvXPEV@technical-crimson-pony.rmq6.cloudamqp.com/csrsgicq";
var bus = RabbitHutch.CreateBus(amqp);
while (true) {
	Console.WriteLine("Press a key to publish a message...");
	Console.ReadKey();
	var greeting = new Greeting() { Name = Environment.MachineName };
	await bus.PubSub.PublishAsync(greeting);
	Console.WriteLine($"Published {greeting}");
}


