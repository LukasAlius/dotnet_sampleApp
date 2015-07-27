using ExampleSetup.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;


namespace ExampleSetup.Controllers
{
    public class HomeController : Controller
    {
        private const string IndividualSummaryUrl ="https://api-beta.direct.id:444/v1/individuals";
        private const string IndividualDetailsUrl = "https://api-beta.direct.id:444/v1/individual/";
        private static string _jsonIndividualsDetails;
        private static string _jsonIndividualsSummary;

        private static string _authenticationToken;

        /// <summary>
        /// Presents a form for populating the credentials required to 
        /// establish a Direct ID API connection.
        /// </summary>
        public ActionResult Index()
        {
            return View(new CredentialsModel());
        }

        /// <summary>
        /// Handles the form post submitted by the <see cref="Index"/> view,
        /// using the supplied credentials
        /// </summary>
        [HttpPost]
        public async Task<ViewResult> Connect(CredentialsModel credentials)
        {
            _authenticationToken = AcquireOAuthAccessToken(credentials);
            var userSessionToken = await AcquireUserSessionToken(_authenticationToken, new Uri(credentials.API));

            return View("Widget", new WidgetModel(userSessionToken, credentials.FullCDNPath));
        }

        /// <summary>
        /// Load a Individuals Summary page
        /// </summary>
        public async Task<ViewResult> IndividualsSummary()
        {
            _jsonIndividualsSummary = await getJson(IndividualSummaryUrl);
            return View(PopulateIndividualsSummaryModel(_jsonIndividualsSummary));
        }

        /// <summary>
        /// Load a Individuals Details page
        /// </summary>
        public async Task<ViewResult> IndividualDetails(string reference)
        {
            _jsonIndividualsDetails = await getJson(IndividualDetailsUrl + reference);
            return View(PopulateIndividualDetailsModel(_jsonIndividualsDetails));
        }

        /// <summary>
        /// Obtains an OAuth access token which can then be used to make authorized calls
        /// to the Direct ID API.
        /// </summary>
        /// <remarks>
        /// <para>The returned value is expected to be included in the authentication header
        /// of subsequent API requests.</para>
        /// <para>As the returned value authenticates the application, API calls made using
        /// this value should only be made using server-side code.</para>
        /// </remarks>
        private static string AcquireOAuthAccessToken(CredentialsModel credentials)
        {
            TrimCredentialsModel(credentials);
            var context = new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext(
                credentials.Authority);

            var accessToken = context.AcquireToken(
                credentials.ResourceID,
                new Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential(
                    credentials.ClientID,
                    credentials.SecretKey));

            if (accessToken == null)
            {
                throw new InvalidOperationException(
                    "Unable to acquire access token from resource: " + credentials.ResourceID +
                    ".  Please check your settings from Direct ID.");
            }

            return accessToken.AccessToken;
        }

        private static void TrimCredentialsModel(CredentialsModel credentials)
        {
            credentials.API = credentials.API.Trim();
            credentials.Authority = credentials.Authority.Trim();
            credentials.ClientID = credentials.ClientID.Trim();
            credentials.ResourceID = credentials.ResourceID.Trim();
            credentials.SecretKey = credentials.SecretKey.Trim();
            credentials.FullCDNPath = credentials.FullCDNPath.Trim();
        }

        /// <summary>
        /// Queries <paramref name="apiEndpoint"/> with an http request
        /// authorized with <paramref name="authenticationToken"/>.
        /// </summary>
        private static async Task<string> AcquireUserSessionToken(
            string authenticationToken,
            Uri apiEndpoint)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authenticationToken);

            var response = await httpClient.GetAsync(apiEndpoint);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    "Unable to acquire access token from endpoint: " + apiEndpoint +
                    ".  Please check your settings from Direct ID.");
            }

            var userSessionResponse = await response.Content.ReadAsAsync<UserSessionResponse>();
            return userSessionResponse.token;
        }

        /// <summary>
        /// Getting Json using authorization token
        /// </summary>
        private async Task<string> getJson(string url)
        {
            // Connecting using a authentication token (OAuthorization)
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authenticationToken);
            string rawJson = await httpClient.GetStringAsync(url);

            return rawJson;
        }

        private static List<IndividualsSummary> PopulateIndividualsSummaryModel(string json)
        {
            var parsedJson = JObject.Parse(json);
            List<IndividualsSummary> individuals = new List<IndividualsSummary>();

            foreach (var item in parsedJson["Individuals"])
            {
                string reference = (string)item["Reference"];
                string timestamp = (string)item["Timestamp"];
                string name = (string)item["Name"];
                string emailAddress = (string)item["EmailAddress"];
                string userId = (string)item["UserID"];
                IndividualsSummary individual = new IndividualsSummary(reference, timestamp, name, emailAddress, userId);
                individuals.Add(individual);
            }

            return individuals;
        }

        private static /*List<*/IndividualDetails/*>*/ PopulateIndividualDetailsModel(string json)
        {
            dynamic parsedJson = JObject.Parse(json);
            string reference = parsedJson.Individual["Reference"];
            var providers = parsedJson.Individual.Global.Bank.Providers[0];
            string provider = providers["Provider"];
            var accountsJson = providers.Accounts;

            var individual = new List<IndividualDetails>();
            var accounts = new List<AccountDetails>();

            GetAccounts(accountsJson, accounts);

            /*individual.Add();*/
            return new IndividualDetails(reference, provider, accounts);
        }

        private static void GetAccounts(dynamic accountsJson, List<AccountDetails> accounts)
        {
            foreach (var item in accountsJson)
            {
                string accountName = (string) item["AccountName"];
                string accountHolder = (string) item["AccountHolder"];
                string accountType = (string) item["AccountType"];
                string activityAvailableFrom = (string) item["ActivityAvailableFrom"];
                string accountNumber = (string) item["AccountNumber"];
                string sortCode = (string) item["SortCode"];
                string balance = (string) item["Balance"];
                string balanceFormatted = (string) item["BalanceFormatted"];
                string currencyCode = (string) item["CurrencyCode"];
                string verifiedOn = (string) item["VerifiedOn"];

                var transactions = new List<Transaction>();
                foreach (var details in item["Transactions"])
                {
                    string date = (string) details["Date"];
                    string description = (string) details["Description"];
                    string amount = (string) details["Amount"];
                    string type = (string) details["Type"];
                    transactions.Add(new Transaction(date, description, amount, type));
                }

                var accountDetails = new AccountDetails(accountName, accountHolder, accountType, activityAvailableFrom,
                    accountNumber, sortCode, balance, balanceFormatted, currencyCode, verifiedOn, transactions);
                accounts.Add(accountDetails);
            }
        }
    }

}