using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ExampleSetup.Models
{
    public class Transaction
    {
        public string Date { get; set; }

        public string Description { get; set; }

        public string Amount { get; set; }

        public string Type { get; set; }

        public Transaction(string date, string description, string amount, string type)
        {
            this.Date = date;
            this.Description = description;
            this.Amount = amount;
            this.Type = type;
        }
    }
}