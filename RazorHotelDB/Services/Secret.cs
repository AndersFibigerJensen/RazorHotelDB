namespace RazorHotelDB.Services
{
    public static class Secret
    {
        private static string _connectionstring = "Data Source=mssql12.unoeuro.com;Initial Catalog=welcome_to_my_domain_dk_db_domain;User ID=welcome_to_my_domain_dk;Password=BcF9ezDfmR2npgyd364w;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static string Connectionstring { get { return _connectionstring; } }


    }
}
