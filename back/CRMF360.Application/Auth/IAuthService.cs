using System.Threading.Tasks;

namespace CRMF360.Application.Auth
{
    public interface IAuthService
    {
        Task<LoginResult?> LoginAsync(string usernameOrEmail, string password);
    }

    public class LoginResult
    {
        public string Token { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
