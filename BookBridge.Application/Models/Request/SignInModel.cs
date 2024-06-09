namespace BookBridge.Application.Models.Request
{
    public class SignInModel
    {
        public required string Password { get; set; }

        public required string UserName { get; set; }
    }
}
