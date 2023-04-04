using SuperShortLink.Helpers;

namespace SuperShortLink.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginModel(string userName, string pwd)
        {
            UserName = userName;
            Password = pwd;
        }
    }

    public class LoginConst
    {
        public static string CacheKey = "LoginCache";
        public static string GetToken(string name, string pwd) => $"{name}&&{pwd}".ToMd5();
    }
}
