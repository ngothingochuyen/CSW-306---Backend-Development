using DATENOW18062026_BE_NgoThiNgocHuyen.Models;

namespace DATENOW18062026_BE_NgoThiNgocHuyen.Repositories
{
    public interface IPetRepository
    {
        Task<List<Pets>> GetAll();
        Task<Pets?> GetById(int id);
        Task Add(Pets Pets);
        Task Update(Pets Pets);
        Task Delete(int id);
        Task<List<Pets>> GetByCategoryId(int categoryId);
    }
}
