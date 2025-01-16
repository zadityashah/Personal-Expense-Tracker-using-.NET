using PersonalExpenseTracker.DataModel.Model;
using PersonalExpenseTracker.DataAccess.Services.Interface;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace PersonalExpenseTracker.Pages
{
    public partial class Debt : ComponentBase
    {
        private string titleFilter = string.Empty;
        private DateTime? startDateFilter = null;
        private DateTime? endDateFilter = null;
        private List<Transaction> allDebts = new List<Transaction>();
        private List<Transaction> filteredDebts = new List<Transaction>();

        [Inject] public ITransactionService TransactionService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadDebts();
        }

        private async Task LoadDebts()
        {
            var debts = await TransactionService.GetAllTransactions();
            allDebts = debts.Where(d => d.transactionType == TransactionType.debt && d.status == "unpaid").ToList();
            filteredDebts = allDebts;
        }

        private void ApplyFilters()
        {
            filteredDebts = allDebts;

            if (!string.IsNullOrWhiteSpace(titleFilter))
            {
                filteredDebts = filteredDebts.Where(d => d.title.Contains(titleFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (startDateFilter.HasValue)
            {
                filteredDebts = filteredDebts.Where(d => d.duedate >= startDateFilter.Value).ToList();
            }

            if (endDateFilter.HasValue)
            {
                filteredDebts = filteredDebts.Where(d => d.duedate <= endDateFilter.Value).ToList();
            }
        }

        private async Task PayDebt(Guid transactionId)
        {
            var success = await TransactionService.PayDebt(transactionId);

            if (success)
            {
                await LoadDebts(); // Refresh the debts list
            }
            else
            {
                Console.WriteLine("Failed to pay debt."); // Handle failure
            }
        }

        private async Task DeleteDebt(Guid transactionId)
        {
            var success = await TransactionService.DeleteTransaction(transactionId);

            if (success)
            {
                await LoadDebts(); // Refresh the debts list
            }
            else
            {
                Console.WriteLine("Failed to delete debt."); // Handle failure
            }
        }
    }
}
