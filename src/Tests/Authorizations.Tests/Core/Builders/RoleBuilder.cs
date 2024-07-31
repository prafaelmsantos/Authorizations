namespace Authorizations.Tests.Core.Builders
{
    public class RoleBuilder
    {
        private static readonly Faker data = new("en");

        public static Role Role()
        {
            return new(data.Company.CompanyName());
        }
        public static RoleDTO RoleDTO()
        {
            return new()
            {
                Id = data.Random.Long(1),
                Name = data.Company.CompanyName(),
                IsDefault = data.Random.Bool(),
                IsReadOnly = data.Random.Bool()
            };
        }
        public static Role Role(RoleDTO dto)
        {
            return new(dto.Name);
        }
        public static Role FullRole(RoleDTO dto)
        {
            return new(dto.Id, dto.Name, dto.IsDefault, dto.IsReadOnly);
        }
        public static List<Role> RoleList(RoleDTO dto)
        {
            return new List<Role>() { Role(dto) };
        }
        public static List<Role> FullRoleList(RoleDTO dto)
        {
            return new List<Role>() { FullRole(dto) };
        }
        public static List<RoleDTO> RoleDTOList(RoleDTO dto)
        {
            return new List<RoleDTO>() { dto };
        }
        public static IQueryable<Role> IQueryable(RoleDTO dto)
        {
            return RoleList(dto).AsQueryable();
        }
        public static IQueryable<Role> FullIQueryable(RoleDTO dto)
        {
            return FullRoleList(dto).AsQueryable();
        }
        public static IQueryable<Role> IQueryableEmpty()
        {
            return new List<Role>().AsQueryable();
        }
    }
}
