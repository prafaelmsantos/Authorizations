namespace Authorizations.Persistence.ServicesRegistry
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            return services.AddPersistenceServices(services.BuildServiceProvider().GetRequiredService<IConfiguration>());
        }
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            string key = Environment.GetEnvironmentVariable("TOKEN_KEY") ?? configuration?.GetValue<string>("TokenKey") ?? "super secret key";

            #region Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            #endregion

            #region Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ITokenService, TokenService>(x => new TokenService(key));

            //JWT
            //Para facilitar a criação de password. Nao Requerer Letras maisculuas, minusculas e numeros. Apenas requer uma password de tamanho 6
            services.AddIdentityCore<User>(x =>
            {
                x.Password.RequireDigit = false;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireLowercase = false;
                x.Password.RequireUppercase = false;
                x.Password.RequiredLength = 6;
            })
            .AddRoles<Role>()
            .AddRoleManager<RoleManager<Role>>()
            .AddSignInManager<SignInManager<User>>()
            .AddRoleValidator<RoleValidator<Role>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();//Se nao adicionar o Default Token, o no UserService, ele não faz o Generate/reset token

            #endregion

            return services;
        }
    }
}
