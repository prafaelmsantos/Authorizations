namespace Authorizations.Core.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<User, UserLoginDTO>().ReverseMap();

            CreateMap<Role, RoleDTO>().ReverseMap();
        }
    }
}
