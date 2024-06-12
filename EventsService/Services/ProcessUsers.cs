
using EventsService.Data;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace EventsService.Services;

public class ProcessUsers : BackgroundService
{
    private readonly IConfiguration _configuration;

    private readonly IServiceProvider _serviceProvider;

    private IConnection? _messageConnection;

    private IModel? _messageChannel;

    private const string ConfigKeyName = "RabbitMQ:UsersQueueName";

    public ProcessUsers(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string queueName = _configuration[ConfigKeyName];
        if (queueName is null)
        {
            return Task.CompletedTask;
        }

        _messageConnection = _serviceProvider.GetService<IConnection>();
        _messageChannel = _messageConnection?.CreateModel();
        _messageChannel.QueueDeclare(queueName, exclusive: false);

        var consumer = new EventingBasicConsumer(_messageChannel);
        consumer.Received += ProcessMessageAsync;
        _messageChannel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        _messageChannel?.Dispose();
    }

    private void ProcessMessageAsync(object? sender, BasicDeliverEventArgs args)
    {
        var message = args.Body;

        var model = JsonSerializer.Deserialize<UserDTO>(message.Span);

        using (var scope = _serviceProvider.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Users.Add(model);
            db.SaveChanges();
        }
    }
}
