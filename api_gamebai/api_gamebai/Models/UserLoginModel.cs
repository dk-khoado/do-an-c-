namespace api_gamebai.Controllers
{
    public class UserLoginModel
    {
        public string username;
        public string password;
        public bool isNull()
        {
            if (username == null || password == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}