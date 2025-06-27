namespace TechnicalService.UserUI.Constants
{
    public static class UserRouteConstants
    {
        //Auth
        public const string Login = "/login";
        public const string Register = "/register";
        public const string Logout = "/logout";
        public const string ForgotPassword = "/forgot-password/";
        public const string ResetPassword = "/reset-password/{0}";
        public const string VerifyEmail = "/verify-email/{0}";

        //profile
        public const string Profile = "/my-profile/";
        public const string EditProfile = "/edit-profile/";
        public const string ChangePassword = "/change-password/";

        public const string UserProducts = "/my-products/";

        //ServiceRecord
        public const string ServiceRecords = "/my-service-records/";
        public const string CreateServiceRecord = "/create-service-record/";
        public const string ServiceRecordStatus = "/service-record-status/{0}";

    }
}
