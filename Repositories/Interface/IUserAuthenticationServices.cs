using MovieStoreMvc.Models.DTO;

namespace MovieStoreMvc.Repositories.Interface
{
    public interface IUserAuthenticationServices
    {
        Task<Status> LoginAsync(Login model);
        Task LogoutAsync();
        Task<Status> RegisterAsync(Registration model);
    }
}
