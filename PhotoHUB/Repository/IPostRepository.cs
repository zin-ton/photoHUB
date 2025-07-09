using PhotoHUB.models;

namespace PhotoHUB.Repository;

public interface IPostRepository : IGenericRepository<Post>
{
    public Task<IEnumerable<Post>> GetPostAsync(int page, int pageSize);
}