namespace PersonalExpenseTracker.DataModel.Model
{
    public class User
    {
        public Guid UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Currency PreferredCurrency { get; set; }
    }
}