using PersonalExpenseTracker.DataModel.Model;
using PersonalExpenseTracker.Services;
using Microsoft.AspNetCore.Components;

namespace PersonalExpenseTracker.Components.Pages
{
    public partial class Login
    {
        private string? ErrorMessage;

        public User Users { get; set; } = new();


        #region Login
        private async void HandleLogin()
        {
            if (await UserService.Login(Users))
            {
                Nav.NavigateTo("/home");
            }
            else
            {
                ErrorMessage = "Invalid username or password.";
            }
        }

        #endregion

    }
}