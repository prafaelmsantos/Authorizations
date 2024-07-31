namespace Authorizations.Persistence.Interfaces.Services
{
    public interface ITokenService
    {
        string CreateToken(UserDTO userDTO);
    }
}
