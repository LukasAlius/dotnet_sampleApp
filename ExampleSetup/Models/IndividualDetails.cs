using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleSetup.Models
{
    public class IndividualDetails
    {
        public string Reference { get; set; }

        public string Provider { get; set; }

        public List<AccountDetails> Accounts { get; set; }

        public string EmailAddress { get; set; }

        public string UserID { get; set; }

        public IndividualDetails(string reference, string timestamp, List<AccountDetails> accounts)
        {
            this.Reference = reference;
            this.Provider = timestamp;
            this.Accounts = accounts;
        }
    }
}