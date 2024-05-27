using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
public interface IRabbitMQConsumerController
{
    void StartConsuming();
    void ConfigQueue(string queueName);
}