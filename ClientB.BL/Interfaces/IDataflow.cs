using System.Threading.Tasks;

namespace ClientB.BL.Interfaces
{
    public interface IDataflow
    {
        Task SendCar(byte[] data);
    }
}
