namespace rgz
{
    internal static class ServerInfo
    {
        private static readonly string host = "localhost";
        private static readonly string port = "5432";
        private static readonly string datebase = "isrgr";
        private static readonly string username = "postgres";
        private static readonly string password = "12345";

        public static string GetConnectionString()
        {
            return
                $"Host = {host}; Port = {port}; Database = {datebase}; Username = {username}; Password = {password};";
        }
    }
}