using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleSetup.Models
{
    public class AccountDetails
    {
        public string AccountName { get; set; }

        public string AccountHolder { get; set; }

        public string AccountType { get; set; }

        public string ActivityAvailableFrom { get; set; }

        public string AccountNumber { get; set; }

        public string SortCode { get; set; }

        public string Balance { get; set; }

        public string BalanceFormatted { get; set; }

        public List<Transaction> Transactions { get; set; }

        public string CurrencyCode { get; set; }

        public string VerifiedOn { get; set; }

        public AccountDetails(string accountName, string accountHolder, string accountType, string activityAvailableFrom, string accountNumber, string sortCode, string balance, string balanceFormatted, string currencyCode, string verifiedOn, List<Transaction> transactions)
        {
            this.AccountName = accountName;
            this.AccountHolder = accountHolder;
            this.AccountType = accountType;
            this.ActivityAvailableFrom = activityAvailableFrom;
            this.AccountNumber = accountNumber;
            this.SortCode = sortCode;
            this.Balance = balance;
            this.BalanceFormatted = balanceFormatted;
            this.CurrencyCode = currencyCode;
            this.VerifiedOn = verifiedOn;
            this.Transactions = transactions;
        }
    }
}