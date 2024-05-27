using RabbitMQ.Client;

using Application.DTO;
using RabbitMQ.Client.Events;
using System.Text;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    public class RabbitMQColabConsumerController : IRabbitMQConsumerController
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private List<string> _errorMessages = new List<string>();

        private string _queueName;
 
        public RabbitMQColabConsumerController(IServiceScopeFactory scopeFactory, IConnectionFactory factory)
        {
            _scopeFactory = scopeFactory;
            _factory = factory;
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "colab_logs", type: ExchangeType.Fanout);
 
            Console.WriteLine(" [*] Waiting for messages from Colaborator.");

            
        }

 
        public void StartConsuming()
        {
            Console.WriteLine("colabs");
            
            var consumer = new EventingBasicConsumer(_channel);
            
            consumer.Received += async (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                //_colaboratorIdService.Add(colaborador);
                var colabResult = JsonConvert.DeserializeObject<ColaboratorIdDTO>(message);
                var colaboratorIDDTO = new ColaboratorIdDTO
                {
                    Id =colabResult.Id,
                };
           
 
 
                using (var scope = _scopeFactory.CreateScope()){
                    var colaboratorIdService = scope.ServiceProvider.GetRequiredService<ColaboratorIdService>();
                    await colaboratorIdService.Add(colaboratorIDDTO, _errorMessages);
                    Console.WriteLine("colaborator created");
                };
            };
            
            _channel.BasicConsume(queue: _queueName,
                                autoAck: true,
                                consumer: consumer);
        }

         public void ConfigQueue(string queueName)
        {
            _queueName = queueName;

            _channel.QueueDeclare(queue: _queueName,
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

            _channel.QueueBind(queue: _queueName,
                  exchange: "colab_logs",
                  routingKey: string.Empty);
        }
 
        public void StopConsuming()
        {
            throw new NotImplementedException();
        }
    }
}