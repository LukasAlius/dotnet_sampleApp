namespace ExampleSetup.Models
{
    /// <summary>
    /// Defines the expected format of the response from an authenticated request
    /// to <see cref="CredentialsModel.API"/>.
    /// </summary>
    public class UserSessionResponse
    {
        /// <summary>
        /// The user session token.
        /// </summary>
        /// <remarks>
        /// This value is expected to be assigned as the value of the data-token
        /// attribute of a #did div element.
        /// </remarks>
        public string token { get; set; }
        public string fullCDNPath { get; set; }
    }

}