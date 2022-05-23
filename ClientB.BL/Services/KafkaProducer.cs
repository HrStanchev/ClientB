using ClientB.BL.Interfaces;
using ClientB.Models;
using Confluent.Kafka;
using System.Threading.Tasks;

namespace ClientB.BL.Services
{
    public class KafkaProducer : IKafkaProducer
    {
        private IProducer<int, Car> _producer;

        public KafkaProducer()
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092"
            };

            _producer = new ProducerBuilder<int, Car>(config)
                            .SetValueSerializer(new MsgPackSerializer<Car>())
                            .Build();
        }
        public async Task ProduceCar(Car car)
        {
            var result = await _producer.ProduceAsync("Cars", new Message<int, Car>()
            {
                Key = car.Id,
                Value = car
            });
        }
    }
}
