using AuthService.Models.DTO;
using RabbitMQ.Client;
using System.Text.Json;

namespace AuthService.Service;

public class MessageSender
{
    private readonly IConnection _messageConnection;
    private readonly IConfiguration _configuration;

    private const string _configKeyName = "RabbitMQ:QueueName";

    public MessageSender(IConfiguration configuration, IConnection messageConnection)
    {
        _configuration = configuration;
        _messageConnection = messageConnection;
    }
    public void SendUserMessage(UserDTO user)
    {
        string? queueName = _configuration[_configKeyName];
        if (queueName is null)
        {
            return;
        }
        using (var channel = _messageConnection.CreateModel())
        {
            channel.QueueDeclare(queueName, exclusive: false);
            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: JsonSerializer.SerializeToUtf8Bytes(user)
            );
        }
    }
}
