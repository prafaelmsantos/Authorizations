namespace Authorizations.Persistence.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(long id);
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<bool> LoginUserAsync(UserLoginDTO userLoginDTO);
        Task<UserDTO> AddUserAsync(UserDTO userDTO);
        Task<UserDTO> UpdateUserAsync(UserDTO userDTO);
        Task<UserDTO> UpdateUserModeAsync(long id, bool mode);
        Task<UserDTO> UpdateUserImageAsync(long id, string? image);
        Task<UserDTO> UpdateUserPasswordAsync(UserLoginDTO userLoginDTO);
        Task<UserDTO> ResetUserPasswordAsync(string userName);
        Task<List<InternalBaseResponseDTO>> DeleteUsersAsync(List<long> usersIds);
    }
}
