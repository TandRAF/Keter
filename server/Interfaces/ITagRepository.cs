using server.Models;

namespace server.Interfaces
{
    public interface ITagRepository
    {
        Task<Tag> CreateTagAsync(Tag tag);
        Task<List<Tag>> GetTagsByProjectIdAsync(Guid projectId);
    }
}