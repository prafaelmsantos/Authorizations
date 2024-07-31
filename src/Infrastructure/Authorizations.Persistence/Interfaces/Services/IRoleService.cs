namespace Authorizations.Persistence.Interfaces.Services
{
    public interface IRoleService
    {
        Task<List<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO> GetRoleByIdAsync(long roleId);
        Task<RoleDTO> AddRoleAsync(RoleDTO roleDTO);
        Task<RoleDTO> UpdateRoleAsync(RoleDTO roleDTO);
        Task<List<InternalBaseResponseDTO>> DeleteRolesAsync(List<long> rolesIds);
    }
}
