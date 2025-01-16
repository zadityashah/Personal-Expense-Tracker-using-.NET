using PersonalExpenseTracker.DataModel.Model;

namespace PersonalExpenseTracker.DataAccess.Services.Interface
{
    public interface IUserService
    {
        Task<bool> Login(User user);
    }
}