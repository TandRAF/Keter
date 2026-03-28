using server.Models;

namespace server.Interfaces
{
    public interface ITagRepository
    {
        Task<Tag> CreateTagAsync(Tag tag);
    }
}