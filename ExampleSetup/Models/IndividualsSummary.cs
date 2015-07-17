using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleSetup.Models
{
    public class IndividualsSummary
    {
        public string Reference { get; set; }

        public string Timestamp { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string UserID { get; set; }

        public IndividualsSummary(string reference, string timestamp, string name, string emailAddress, string userID)
        {
            this.Reference = reference;
            this.Timestamp = timestamp;
            this.Name = name;
            this.EmailAddress = emailAddress;
            this.UserID = userID;
        }
    }
}