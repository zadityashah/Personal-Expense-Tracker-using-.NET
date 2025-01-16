using System;

namespace PersonalExpenseTracker.DataModel.Model
{
    public class Transaction
    {
        public Guid transactionId { get; set; }
        public Tag tag { get; set; }

        public Guid tagId { get; set; }
        public DateTime date { get; set; }
        public TransactionType transactionType { get; set; }
        public string title { get; set; }
        public double amount { get; set; }
        public string notes { get; set; }
        public DateTime? duedate { get; set; } // Nullable DateTime for debt transactions
        public double remainingBalance { get; set; }

        public string? status { get; set; } = "unpaid";
    }
}