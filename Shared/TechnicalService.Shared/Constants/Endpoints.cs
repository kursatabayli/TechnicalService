namespace TechnicalService.Shared.Constants
{
    public static class Endpoints
    {
        public const string BaseUrl = "https://localhost:7278/";
        //public const string BaseUrl = "https://api.teknikservisapp.com/";


        //UserAuth endpoints
        public const string UserAuthBaseUrl = "api/UserAuth";
        public const string UserLogin = $"{UserAuthBaseUrl}/Login/";
        public const string UserLogout = $"{UserAuthBaseUrl}/Logout/";
        public const string UserRefreshToken = $"{UserAuthBaseUrl}/RefreshToken/";
        public const string UserRequestPasswordReset = $"{UserAuthBaseUrl}/RequestPasswordReset/";

        //PersonnelAuth endpoints
        public const string PersonnelAuthBaseUrl = "api/PersonnelAuth";
        public const string PersonnelLogin = $"{PersonnelAuthBaseUrl}/Login/";
        public const string PersonnelLogout = $"{PersonnelAuthBaseUrl}/Logout/";
        public const string PersonnelChangePassword = $"{PersonnelAuthBaseUrl}/ChangePassword/";
        public const string PersonnelRefreshToken = $"{PersonnelAuthBaseUrl}/RefreshToken/";
        public const string PersonnelRequestPasswordReset = $"{PersonnelAuthBaseUrl}/RequestPasswordReset/";

        //CheckSession endpoints
        public const string CheckSession = "api/Session/Check";

        //Register endpoints
        public const string Register = "api/Register";
        public const string VerifyEmail = $"{Register}/verify-email/";
        public const string ResendEmail = $"{Register}/resend-email/";

        //User endpoints
        public const string UserBaseUrl = "api/User";
        public const string GetUser = $"{UserBaseUrl}/CurrentUser";
        public const string GetUserById = $"{UserBaseUrl}/GetUserById/";
        public const string UserChangePassword = $"{UserBaseUrl}/ChangePassword/";
        public const string ResetPassword = $"{UserBaseUrl}/ResetPassword/";

        //User Products endpoints
        public const string UserProductsBaseUrl = "api/UserProduct";
        public const string GetUserProducts = $"{UserProductsBaseUrl}/GetUserProducts/";
        public const string GetUserProductByUserProductId = $"{UserProductsBaseUrl}/GetUserProductByUserProductId/";
        public const string AddUserProduct = $"{UserProductsBaseUrl}/AddUserProduct/";
        public const string GetUserProductById = $"{UserProductsBaseUrl}/GetUserProductById/";

        // Service Record endpoints
        public const string ServiceRecordBaseUrl = "api/ServiceRecord";
        public const string GetAllServiceRecords = $"{ServiceRecordBaseUrl}/GetAllServiceRecords/";
        public const string GetServiceRecordById = $"{ServiceRecordBaseUrl}/GetServiceRecordById/";
        public const string GetServiceRecordDetail = $"{ServiceRecordBaseUrl}/GetServiceRecordDetail/";
        public const string GetServiceRecordsByPersonnelId = $"{ServiceRecordBaseUrl}/GetServiceRecordsByPersonnelId/";
        public const string GetServiceRecordsByServiceId = $"{ServiceRecordBaseUrl}/GetServiceRecordsByServiceId/";
        public const string CreateServiceRecord = $"{ServiceRecordBaseUrl}/CreateServiceRecord/";
        public const string GetUserServiceRecordsByUser = $"{ServiceRecordBaseUrl}/GetUserServiceRecordsByUserId/";
        public const string UpdateServiceRecord = $"{ServiceRecordBaseUrl}/UpdateServiceRecord/";
        public const string SearchServiceRecord = $"{ServiceRecordBaseUrl}/SearchServiceRecord/";

        // Service Record Step endpoints
        public const string ServiceRecordStepBaseUrl = "api/ServiceRecordStep";
        public const string GetServiceRecordStepsByServiceRecordId = $"{ServiceRecordStepBaseUrl}/GetServiceRecordStepsByServiceRecordId/";
        public const string AddServiceRecordStep = $"{ServiceRecordStepBaseUrl}/AddServiceRecordStep/";
        public const string GetServiceRecordStepById = $"{ServiceRecordStepBaseUrl}/GetServiceRecordStepById/";
        public const string UpdateServiceRecordStep = $"{ServiceRecordStepBaseUrl}/UpdateServiceRecordStep/";

        // Technical Service endpoints
        public const string TechnicalServiceBaseUrl = "api/TechnicalService";
        public const string GetAllTechnicalServices = $"{TechnicalServiceBaseUrl}/GetAllTechnicalServices/";
        public const string GetTechnicalServiceById = $"{TechnicalServiceBaseUrl}/GetTechnicalServiceById/";
        public const string CreateTechnicalService = $"{TechnicalServiceBaseUrl}/CreateTechnicalService/";
        public const string UpdateTechnicalService = $"{TechnicalServiceBaseUrl}/UpdateTechnicalService/";
        public const string DeleteTechnicalService = $"{TechnicalServiceBaseUrl}/DeleteTechnicalService/";

        // Brand endpoints
        public const string BrandBaseUrl = "api/Brand";
        public const string GetAllBrands = $"{BrandBaseUrl}/GetAllBrands/";
        public const string GetBrandById = $"{BrandBaseUrl}/GetBrandById/";
        public const string CreateBrand = $"{BrandBaseUrl}/CreateBrand/";
        public const string UpdateBrand = $"{BrandBaseUrl}/UpdateBrand/";
        public const string DeleteBrand = $"{BrandBaseUrl}/DeleteBrand/";


        //Personnel endpoints
        public const string PersonnelBaseUrl = "api/Personnel";
        public const string GetAllPersonnels = $"{PersonnelBaseUrl}/GetAllPersonnels/";
        public const string GetPersonnelsByService = $"{PersonnelBaseUrl}/GetPersonnelsByService/";
        public const string GetPersonnelById = $"{PersonnelBaseUrl}/GetPersonnelById/";
        public const string CurrentPersonnel = $"{PersonnelBaseUrl}/CurrentPersonnel/";
        public const string CreatePersonnel = $"{PersonnelBaseUrl}/CreatePersonnel";
        public const string UpdatePersonnel = $"{PersonnelBaseUrl}/UpdatePersonnel/";
        public const string DeletePersonnel = $"{PersonnelBaseUrl}/DeletePersonnel/";

        // Product Type endpoints
        public const string ProductTypeBaseUrl = "api/ProductType";
        public const string GetAllProductTypes = $"{ProductTypeBaseUrl}/GetAllProductTypes/";
        public const string GetProductTypeById = $"{ProductTypeBaseUrl}/GetProductTypeById/";
        public const string CreateProductType = $"{ProductTypeBaseUrl}/CreateProductType/";
        public const string UpdateProductType = $"{ProductTypeBaseUrl}/UpdateProductType/";
        public const string DeleteProductType = $"{ProductTypeBaseUrl}/DeleteProductType/";

        //Product endpoints
        public const string ProductBaseUrl = "api/Product";
        public const string GetAllProducts = $"{ProductBaseUrl}/GetAllProducts/";
        public const string GetProductById = $"{ProductBaseUrl}/GetProductById/";
        public const string CreateProduct = $"{ProductBaseUrl}/CreateProduct/";
        public const string UpdateProduct = $"{ProductBaseUrl}/UpdateProduct/";
        public const string DeleteProduct = $"{ProductBaseUrl}/DeleteProduct/";

        //Legal Document endpoints
        public const string LegalDocumentBaseUrl = "api/LegalDocument";
        public const string GetAllLegalDocuments = $"{LegalDocumentBaseUrl}/GetAllLegalDocuments/";
        public const string GetLegalDocumentById = $"{LegalDocumentBaseUrl}/GetLegalDocumentById/";
        public const string CreateLegalDocument = $"{LegalDocumentBaseUrl}/CreateLegalDocument/";
        public const string UpdateLegalDocument = $"{LegalDocumentBaseUrl}/UpdateLegalDocument/";
        public const string GetLegalDocumentByDocumentType = $"{LegalDocumentBaseUrl}/GetLegalDocumentByDocumentType/";


        //Serial Number endpoints
        public const string SerialNumberBaseUrl = "api/SerialNumber";
        public const string GetAllSerialNumbers = $"{SerialNumberBaseUrl}/GetAllSerialNumbers/";
        public const string GetSerialNumberById = $"{SerialNumberBaseUrl}/GetSerialNumberById/";
        public const string GetSerialNumberBySerialNumber = $"{SerialNumberBaseUrl}/GetSerialNumberBySerialNumber/";
        public const string CreateSerialNumber = $"{SerialNumberBaseUrl}/CreateSerialNumber/";
        public const string UpdateSerialNumber = $"{SerialNumberBaseUrl}/UpdateSerialNumber/";
        public const string DeleteSerialNumber = $"{SerialNumberBaseUrl}/DeleteSerialNumber/";


    }
}
