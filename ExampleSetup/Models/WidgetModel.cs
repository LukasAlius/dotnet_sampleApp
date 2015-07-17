using System;
using System.Collections.Generic;
using System.Web;

namespace ExampleSetup.Models
{
    public class WidgetModel
    {
        public string Token { get; private set; }

        public string FullCDNPath { get; private set; }

        public WidgetModel(string token, string fullCDNPath)
        {
            this.FullCDNPath = fullCDNPath;
            this.Token = token;
        }
    }
}