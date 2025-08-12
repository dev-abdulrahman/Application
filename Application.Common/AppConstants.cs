namespace Application.Common
{
    public class AppConstants
    {
        public const string ApplicationLogKey = "ApplicationLog";
        public const string AppConnectionStringKey = "AppConnectionString";

        #region VIEW PATH CONSTANTS

        public const string ErrorView = "~/Views/Error/Error.cshtml";
        public const string NotFoundView = "~/Views/Error/NotFound.cshtml";
        public const string NotAvailableView = "~/Views/Error/NotAvailable.cshtml";
        public const string AlreadyReturnedView = "~/Views/Error/AlreadyReturned.cshtml";

        #endregion

        #region SUCCESS OR FAIL MESSAGES

        public const string BookAddedSuccess = "Successfully added the book";
        public const string BookAddedFail = "Failed to add the book";

        #endregion

        #region EXTERNAL APIs OR KEYS
        
        public const string LogflareSourceId = "b32c79e7-5485-4119-b74f-454954737e81";
        public const string LogflareAPIKey = "tLQx79ook8iT";

        #endregion
    }
}
