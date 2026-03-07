namespace FrontApplication.Core
{
    public static class ApiEndpoints
    {
        public const string UserBaseAddress = "https://localhost:7270/api/";
        public const string ProductBaseAddress = "https://localhost:7070/api/";
        public const string UserApi = "UserApi";
        public const string ProductApi = "ProductApi";
        public const string ProductGetAll = "Product/GetAll";
        public const string ProductGetById = "Product/GetById?id={0}";
        public const string ProductCreate = "Product/Create";
        public const string ProductUpdate = "Product/Update?id={0}";
        public const string ProductDelete = "Product/Delete?id={0}";
        public const string UserLogin = "User/Login";
        public const string UserRegister = "User/Register";
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
