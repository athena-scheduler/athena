namespace Athena.Models.Login
{
    public class CreateUserViewModel
    {
        public string Name { get; set; }
        public string ReturnUrl { get; set; }
        public string LoginProvider { get; set; }
        public string Email { get; set; }
    }
}