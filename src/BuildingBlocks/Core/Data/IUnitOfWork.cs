using System.Threading.Tasks;

namespace Core.Data
{
    public interface IUnitOfWork
    {
         Task<bool> CommitAsync();
    }
}