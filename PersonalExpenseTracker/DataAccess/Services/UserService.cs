using PersonalExpenseTracker.DataAccess.Services.Interface;
using PersonalExpenseTracker.DataModel.Model;
using PersonalExpenseTracker.DataModel.Seeding;


namespace PersonalExpenseTracker.DataAccess.Services
{
    public class UserService : UserBase,IUserService
    {
        private List<User> _users;

        public UserService()
        {
            _users = LoadUsers();

            // Add default admin user if the file is empty
            if (!_users.Any())
            {
                _users.Add(new User { Username = Seeding.SeedUsername, Password = Seeding.SeedPassword, PreferredCurrency = new Currency("USD") });
                SaveUsers(_users);
            }
        }

        public async Task<bool> Login(User user)
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return false;
            }
            return _users.Any(u => u.Username == user.Username && u.Password == user.Password);
        }
    }
}