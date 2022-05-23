using ClientB.Models;
using System.Threading.Tasks;

namespace ClientB.BL.Interfaces
{
    public interface IKafkaProducer
    {
        Task ProduceCar(Car car);
    }
}
