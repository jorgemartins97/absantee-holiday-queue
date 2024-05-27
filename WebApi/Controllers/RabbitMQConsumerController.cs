using RabbitMQ.Client;

using Application.DTO;
using RabbitMQ.Client.Events;
using System.Text;
using Application.Services;
using Newtonsoft.Json;
 
namespace WebApi.Controllers
{
    public class RabbitMQConsumerController : IRabbitMQConsumerController
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private string _queueName;
        private List<string> _errorMessages = new List<string>();
 
        public RabbitMQConsumerController(IServiceScopeFactory scopeFactory, IConnectionFactory factory)
        {
            _scopeFactory = scopeFactory;
            _factory = factory;
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
 
            _channel.ExchangeDeclare(exchange: "holiday_logs", type: ExchangeType.Fanout);
 
            Console.WriteLine(" [*] Waiting for messages from holiday.");
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
                  exchange: "holiday_logs",
                  routingKey: string.Empty);
        }

        public void StartConsuming()
        {
            Console.WriteLine("holidays");
           
            var consumer = new EventingBasicConsumer(_channel);
           
            consumer.Received += async (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                //_colaboratorIdService.Add(colaborador);
                var holidayResult = JsonConvert.DeserializeObject<HolidayDTO>(message);
                var holidayDTO = new HolidayDTO
                {
                    Id = holidayResult.Id,
                    _colabId = holidayResult._colabId,
                    _holidayPeriod = holidayResult._holidayPeriod
                };
           
 
 
                using (var scope = _scopeFactory.CreateScope()){
                    var holidayService = scope.ServiceProvider.GetRequiredService<HolidayService>();
                    await holidayService.Add(holidayDTO, _errorMessages);
                    Console.WriteLine("holiday criada");
                };
            };
            _channel.BasicConsume(queue: _queueName,
                                autoAck: true,
                                consumer: consumer);
       
        }
 
        public void StopConsuming()
        {
            throw new NotImplementedException();
        }
    }
}