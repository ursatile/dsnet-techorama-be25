using Autobarn.Pricing;
using Autobarn.PricingClient;
using EasyNetQ;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
var amqp = builder.Configuration.GetConnectionString("AutobarnRabbitMQ");
var bus = RabbitHutch.CreateBus(amqp);

using var channel = GrpcChannel.ForAddress("https://grpc.autobarn.dev");
var client = new Pricer.PricerClient(channel);
builder.Services.AddSingleton(client);
builder.Services.AddSingleton(bus);
builder.Services.AddHostedService<AutobarnPricingClientService>();
var host = builder.Build();
await host.RunAsync();