using PersonalExpenseTracker.DataModel.Model;

namespace PersonalExpenseTracker.Services
{
    public class AuthenticationStateService
    {
        private Currency currency;
        private User authenticatedUser;

        public User GetAuthenticatedUser()
        {
            return authenticatedUser;
        }

        public void SetAuthenticatedUser(User user)
        {
            authenticatedUser = user;
        }

        public bool IsAuthenticated()
        {
            if (authenticatedUser != null)
            {
                return true;
            }
            return false;
        }

        public void Logout()
        {
            authenticatedUser = null;
        }
        public Currency GetUserCurrency()
        {
            return currency;
        }

        public void SetCurrency(Currency currency)
        {
            this.currency = currency;
        }
    }
}