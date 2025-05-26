using EasyNetQ;
using Messages;

Console.WriteLine("Welcome to the subscriber!");
var amqp = "amqps://csrsgicq:7PAjB9tTm2zcajhlCPr4y3uxqtGvXPEV@technical-crimson-pony.rmq6.cloudamqp.com/csrsgicq";
var bus = RabbitHutch.CreateBus(amqp);
const string SUBSCRIBER_ID = "dylan";
await bus.PubSub.SubscribeAsync<Greeting>(SUBSCRIBER_ID, message => {
	Console.WriteLine(message);
});

Console.WriteLine("Listening for greetings. Press any key to exit");
Console.ReadKey();
