using PersonalExpenseTracker.DataModel.Model;
using PersonalExpenseTracker.DataAccess.Services.Interface;
using System.Text.Json;

namespace PersonalExpenseTracker.DataAccess.Services
{
    public class TransactionService : ITransactionService
    {
        private List<Transaction> _transactions;

        public TransactionService()
        {
            _transactions = LoadTransactions();
            FixDuplicateTransactionIds(); // Ensure unique IDs on initialization
        }

        public async Task<List<Transaction>> GetAllTransactions()
        {
            return await Task.FromResult(_transactions);
        }

        public async Task<bool> AddTransaction(Transaction transaction)
        {
            // Ensure the transaction has a unique ID
            transaction.transactionId = Guid.NewGuid();

            var currentBalance = await GetBalance();

            // Ensure there is enough balance for debit transactions
            if (transaction.transactionType == TransactionType.debit)
            {
                if (transaction.amount > currentBalance)
                {
                    return false; // Insufficient balance for debit
                }
            }

            // Handle credit transaction
            if (transaction.transactionType == TransactionType.credit)
            {
                transaction.remainingBalance = currentBalance + transaction.amount;
            }
            // Handle debit transaction
            else if (transaction.transactionType == TransactionType.debit)
            {
                transaction.remainingBalance = currentBalance - transaction.amount;
            }
            // Handle debt transaction
            else if (transaction.transactionType == TransactionType.debt)
            {
                transaction.remainingBalance = currentBalance + transaction.amount; // Add debt amount to balance
                transaction.status = "unpaid";  // Explicitly set status to "unpaid" for debt transactions
            }

            _transactions.Add(transaction);  // Add the transaction to the list
            SaveTransactions(_transactions);  // Save the updated transactions list
            return true;  // Successfully added the transaction
        }

        public async Task<double> GetBalance()
        {
            // Calculate the total credits
            var totalCredits = _transactions
                .Where(t => t.transactionType == TransactionType.credit)
                .Sum(t => t.amount);

            // Calculate the total debits
            var totalDebits = _transactions
                .Where(t => t.transactionType == TransactionType.debit)
                .Sum(t => t.amount);

            // Calculate only the unpaid debts
            var unpaidDebts = _transactions
                .Where(t => t.transactionType == TransactionType.debt && t.status == "unpaid")
                .Sum(t => t.amount);

            // Calculate the balance
            var balance = totalCredits - totalDebits + unpaidDebts;

            return await Task.FromResult(balance);
        }


        public async Task<bool> DeleteTransaction(Guid transactionId)
        {
            // Find the transaction with the specified ID
            var transactionToDelete = _transactions.FirstOrDefault(t => t.transactionId == transactionId);

            if (transactionToDelete == null)
            {
                return false; // Transaction not found
            }

            _transactions.Remove(transactionToDelete);
            SaveTransactions(_transactions); // Save the updated list
            return true; // Successfully deleted the transaction
        }

        public async Task<bool> PayDebt(Guid transactionId)
        {
            // Find the debt transaction
            var debtTransaction = _transactions.FirstOrDefault(t => t.transactionId == transactionId && t.transactionType == TransactionType.debt);

            if (debtTransaction == null)
            {
                return false; // Debt transaction not found
            }

            // Get the current balance
            var currentBalance = await GetBalance();

            // Ensure there is enough balance to pay the debt
            if (currentBalance < debtTransaction.amount)
            {
                return false; // Insufficient balance to pay the debt
            }

            // Create a new debit transaction to deduct the debt amount
            var paymentTransaction = new Transaction
            {
                transactionId = Guid.NewGuid(),
                title = $"Debt Payment: {debtTransaction.title}",
                amount = debtTransaction.amount,
                transactionType = TransactionType.debit,
                remainingBalance = currentBalance - debtTransaction.amount,
                date = DateTime.Now
            };

            // Add the payment transaction to the transaction list
            _transactions.Add(paymentTransaction);

            // Update the debt transaction status
            debtTransaction.status = "Paid";

            // Save the updated transaction list
            SaveTransactions(_transactions);

            return true; // Debt payment successful
        }

        private List<Transaction> LoadTransactions()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "transactions.json");

            if (!File.Exists(filePath))
            {
                return new List<Transaction>();
            }

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
        }

        private void SaveTransactions(List<Transaction> transactions)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "transactions.json");
            var json = JsonSerializer.Serialize(transactions);
            File.WriteAllText(filePath, json);
        }

        private void FixDuplicateTransactionIds()
        {
            var uniqueIds = new HashSet<Guid>();
            foreach (var transaction in _transactions)
            {
                // Check if the ID is already in the set
                if (!uniqueIds.Add(transaction.transactionId))
                {
                    // Generate a new unique ID if a duplicate is found
                    transaction.transactionId = Guid.NewGuid();
                }
            }

            // Save the transactions with fixed IDs
            SaveTransactions(_transactions);
        }
    }
}
