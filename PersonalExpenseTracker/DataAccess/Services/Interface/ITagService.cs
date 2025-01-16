using PersonalExpenseTracker.DataModel.Model;

namespace PersonalExpenseTracker.DataAccess.Services.Interface
{
    public interface ITagService
    {
        Task AddTag(Tag tag);
        Task<List<Tag>> GetAllTags();
    }
}