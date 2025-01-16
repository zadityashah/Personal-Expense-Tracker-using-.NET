using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpenseTracker.DataModel.Model
{
    public class Currency
    {
        public Guid CurrencyID { get; set; }
        public string CurrencyName { get; set; }

        public Currency(string currencyName)
        {
            CurrencyID = Guid.NewGuid();
            CurrencyName = currencyName;
        }
        public static List<Currency> GetAvailableCurrencies()
        {
            return new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("JPY"),
                new Currency("GBP"),
                new Currency("AUD")
            };
        }
    }
}