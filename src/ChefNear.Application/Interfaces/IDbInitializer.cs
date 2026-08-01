using System.Threading.Tasks;

namespace ChefNear.Application.Interfaces
{
    public interface IDbInitializer
    {
        Task InitializeAsync();
    }
}
