using Autobarn.Notifier;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
var amqp = builder.Configuration.GetConnectionString("AutobarnRabbitMQ");
var bus = RabbitHutch.CreateBus(amqp);

builder.Services.AddSingleton(bus);
builder.Services.AddHostedService<AutobarnNotifierService>();
var host = builder.Build();
await host.RunAsync();
