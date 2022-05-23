using ClientB.BL.Interfaces;
using ClientB.Models;
using MessagePack;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ClientB.BL.Services
{
    public class Dataflow : IDataflow
    {
        private IKafkaProducer _producer;
        

        TransformBlock<byte[], Car> entryBlock = new TransformBlock<byte[], Car>(data => MessagePackSerializer.Deserialize<Car>(data));

        Random rnd = new Random();

        public Dataflow(IKafkaProducer producer)
        {
            _producer = producer;

            var enrichBlock = new TransformBlock<Car, Car>(c =>
            {
                Console.WriteLine($"Received value: {c.Year}");
                c.Year = rnd.Next(1990, DateTime.Now.Year);

                return c;
            });

            var publishBlock = new ActionBlock<Car>(car =>
            {
                Console.WriteLine($"Updated value: {car.Year} \n");
                _producer.ProduceCar(car);
            });

            var linkOptions = new DataflowLinkOptions()
            {
                PropagateCompletion = true
            };

            entryBlock.LinkTo(enrichBlock, linkOptions);
            enrichBlock.LinkTo(publishBlock, linkOptions);

        }
        public async Task SendCar(byte[] data)
        {          
            await entryBlock.SendAsync(data);
        }  
    }
}
