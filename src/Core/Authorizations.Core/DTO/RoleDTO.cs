namespace Authorizations.Core.DTO
{
    public class RoleDTO
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;
        public bool IsDefault { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
