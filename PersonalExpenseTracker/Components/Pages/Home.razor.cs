using Microsoft.AspNetCore.Components;
using PersonalExpenseTracker.DataModel.Model;
using PersonalExpenseTracker.DataAccess.Services.Interface;

namespace PersonalExpenseTracker.Pages
{
    public partial class Home : ComponentBase
    {
        private List<Transaction> creditTransactions = new List<Transaction>();
        private List<Transaction> debitTransactions = new List<Transaction>();
        private List<Transaction> debtTransactions = new List<Transaction>();

        [Inject] public ITransactionService TransactionService { get; set; }
    }
}