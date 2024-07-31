namespace Authorizations.Persistence.Services
{
    public class UserService : IUserService
    {
        #region Private variables

        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly INotificationsGrpcClientService _notificationsGrpcClientService;

        #endregion

        #region Constructors
        public UserService(
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            INotificationsGrpcClientService notificationsGrpcClientService
            )
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;

            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _notificationsGrpcClientService = notificationsGrpcClientService;
        }

        #endregion

        #region Public methods

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            try
            {
                List<User> users = await _userRepository
                    .GetAll()
                    .Include(x => x.Roles)
                    .OrderBy(x => x.Id).ToListAsync();

                return _mapper.Map<List<UserDTO>>(users);
            }
            catch (Exception ex)
            {
                throw new Exception($"{DomainResources.GetAllUsersAsyncException} {ex.Message}");
            }
        }

        public async Task<UserDTO> GetUserByIdAsync(long id)
        {
            try
            {
                User? user = await _userRepository.GetAll()
                    .Where(x => x.Id == id)
                    .Include(x => x.Roles)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                user.ThrowIfNull(() => throw new Exception(DomainResources.UserNotFoundException));

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"{DomainResources.GetUserByIdAsyncException} {ex.Message}");
            }
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            try
            {
                User? user = await _userRepository
                    .GetAll()
                    .Where(x => x.Email == email)
                    .Include(x => x.Roles)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                user.ThrowIfNull(() => throw new Exception(DomainResources.UserNotFoundException));

                UserDTO userDTO = _mapper.Map<UserDTO>(user);

                return userDTO;
            }
            catch (Exception ex)
            {
                throw new Exception($"{DomainResources.GetUserByEmailAsyncException} {ex.Message}");
            }
        }

        public async Task<bool> LoginUserAsync(UserLoginDTO userLoginDTO)
        {
            try
            {
                User? user = await _userManager
                   .Users
                   .SingleOrDefaultAsync(user => user.Email == userLoginDTO.Email);

                user.ThrowIfNull(() => throw new Exception(DomainResources.UserNotFoundException));

                SignInResult signInResult = await _signInManager
                    .CheckPasswordSignInAsync(user, userLoginDTO.Password, false);

                return signInResult.Succeeded;
            }
            catch (Exception ex)
            {
                throw new Exception($"{DomainResources.LoginUserAsyncException} {ex.Message}");
            }
        }

        public async Task<UserDTO> AddUserAsync(UserDTO userDTO)
        {
            try
            {
                UserExistsAsync(userDTO.Id, userDTO.Email).Result
                    .Throw(() => throw new Exception(DomainResources.UserAlreadyExistsException))
                    .IfTrue();

                User user = new(userDTO.Email, userDTO.PhoneNumber, userDTO.FirstName, userDTO.LastName);

                user = await UpdateRoleAsync(user, userDTO);

                string password = GenerateNewPassword();

                IdentityResult identityResult = await _userManager.CreateAsync(user, password);

                identityResult.Succeeded.Throw(() => throw new Exception(DomainResources.UserNotFoundException))
                    .IfFalse();

                await _notificationsGrpcClientService.SendWelcomeEmailAsync(new SendWelcomeEmailRequestGrpc()
                {
                    Name = $"{user.FirstName} {user.LastName}",
                    Address = user.Email,
                    Password = password
                });

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"{DomainResources.AddUserAsyncException} {ex.Message}");
            }
        }

        public async Task<UserDTO> UpdateUserAsync(UserDTO userDTO)
        {
            try
            {
                User? user = await _userRepository
                    .GetAll()
                    .Where(x => x.Id == userDTO.Id)
                    .Include(x => x.Roles)
                    .FirstOrDefaultAsync();

                user.ThrowIfNull(() => throw new Exception(DomainResources.UserNotFoundException));

                UserExistsAsync(user.Id, user.Email).Result
                    .Throw(() => throw new Exception(DomainResources.UserAlreadyExistsException))
                    .IfTrue();

                user.Update(userDTO.Email, userDTO.PhoneNumber, userDTO.FirstName, userDTO.LastName, userDTO.Image);

                if (user.IsDefault)
                {
                    user = await UpdateWithAdminRoleAsync(user);
                }
                else
                {
                    user = await UpdateRoleAsync(user, userDTO);
                }

                if (userDTO.Password != null)
                {
                    await UpdatePasswordAsync(user, userDTO.Password);
                }

                await _userRepository.UpdateAsync(user);

                await _notificationsGrpcClientService.SendUserProfileUpdatedEmailAsync(new SendUserProfileUpdatedEmailRequestGrpc()
                {
                    Name = $"{user.FirstName} {user.LastName}",
                    Address = user.Email
                });

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"{DomainResources.UpdateUserAsyncException} {ex.Message}");
            }
        }

        public async Task<UserDTO> UpdateUserModeAsync(long id, bool mode)
        {
            try
            {
                User? user = await _userRepository.FindByIdAsync(id);

                user.ThrowIfNull(() => throw new Exception(DomainResources.UserNotFoundException));

                user.SetDarkMode(mode);

                await _userRepository.UpdateAsync(user);

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"{DomainResources.UpdateUserModeAsyncException} {ex.Message}");
            }
        }

        public async Task<UserDTO> UpdateUserImageAsync(long id, string? image)
        {
            try
            {
                User? user = await _userRepository.FindByIdAsync(id);

                user.ThrowIfNull(() => throw new Exception(DomainResources.UserNotFoundException));

                user.SetImage(image);

                await _userRepository.UpdateAsync(user);

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"{DomainResources.UpdateUserImageAsyncException} {ex.Message}");
            }
        }

        public async Task<UserDTO> UpdateUserPasswordAsync(UserLoginDTO userLoginDTO)
        {
            try
            {
                User? user = await _userRepository
                    .GetAll()
                    .Where(x => x.Email == userLoginDTO.Email)
                    .FirstOrDefaultAsync();

                user.ThrowIfNull(() => throw new Exception(DomainResources.UserNotFoundException));

                await UpdatePasswordAsync(user, userLoginDTO.Password);

                await _notificationsGrpcClientService.SendPasswordChangedEmailAsync(new SendPasswordChangedEmailRequestGrpc()
                {
                    Name = $"{user.FirstName} {user.LastName}",
                    Address = user.Email,
                    Password = userLoginDTO.Password
                });

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"{DomainResources.UpdateUserPasswordAsyncException} {ex.Message}");
            }
        }

        public async Task<UserDTO> ResetUserPasswordAsync(string email)
        {
            try
            {
                User? user = await _userRepository
                    .GetAll()
                    .Where(x => x.Email == email)
                    .FirstOrDefaultAsync();

                user.ThrowIfNull(() => throw new Exception(DomainResources.UserNotFoundException));

                var password = GenerateNewPassword();

                await UpdatePasswordAsync(user, password);

                await _notificationsGrpcClientService.SendPasswordResetEmailAsync(new SendPasswordResetEmailRequestGrpc()
                {
                    Name = $"{user.FirstName} {user.LastName}",
                    Address = user.Email,
                    Password = password
                });

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"{DomainResources.ResetUserPasswordAsyncException} {ex.Message}");
            }
        }

        public async Task<List<InternalBaseResponseDTO>> DeleteUsersAsync(List<long> usersIds)
        {
            return await DeleteAsync(usersIds);
        }

        #endregion

        #region Private methods

        private async Task<bool> UserExistsAsync(long id, string email)
        {
            return await _userRepository
                .GetAll()
                .AnyAsync(user => user.Id != id && user.Email.Trim().ToLower() == email.Trim().ToLower());
        }

        private async Task<User> UpdateRoleAsync(User user, UserDTO userDTO)
        {
            (userDTO.Roles.Count == 0)
                .Throw(() => throw new Exception(DomainResources.RoleNotFoundException))
                .IfTrue();

            Role? role = await _roleRepository
                .FindByIdAsync(userDTO.Roles.FirstOrDefault()?.Id ?? 0);

            role.ThrowIfNull(() => throw new Exception(DomainResources.RoleNotFoundException));

            user.SetRoles(new List<Role> { role });

            return user;
        }

        private async Task<User> UpdateWithAdminRoleAsync(User user)
        {

            Role? role = await _roleRepository
                .GetAll()
                .Where(x => x.IsDefault && x.IsReadOnly)
                .FirstOrDefaultAsync();

            role.ThrowIfNull(() => throw new Exception(DomainResources.DefaultRoleNotFoundException));

            user.SetRoles(new List<Role> { role });

            return user;
        }

        private async Task UpdatePasswordAsync(User user, string password)
        {
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, token, password);

            identityResult.Succeeded
                    .Throw(() => throw new Exception(DomainResources.UpdateUserPasswordAsyncException))
                    .IfFalse();
        }

        private static string GenerateNewPassword()
        {
            Random rnd = new();
            return rnd.Next(100000, 1000000000).ToString();
        }

        private async Task<List<InternalBaseResponseDTO>> DeleteAsync(List<long> usersIds)
        {
            List<InternalBaseResponseDTO> responseMessageDTOs = new();

            foreach (long userId in usersIds)
            {
                InternalBaseResponseDTO responseMessageDTO = new() { Id = userId, Success = false };
                try
                {
                    User? user = await _userRepository.FindByIdAsync(userId);

                    if (user is not null)
                    {
                        if (user.IsDefault)
                        {
                            responseMessageDTO.ErrorMessage = DomainResources.DeleteDefaultUserAsyncException;
                        }
                        else
                        {
                            responseMessageDTO.Success = await _userRepository.RemoveAsync(user);
                        }
                    }
                    else
                    {
                        responseMessageDTO.ErrorMessage = DomainResources.UserNotFoundException;
                    }
                }

                catch (Exception)
                {
                    responseMessageDTO.ErrorMessage = DomainResources.DeleteUsersAsyncException;
                }

                responseMessageDTOs.Add(responseMessageDTO);
            }

            return responseMessageDTOs;
        }

        #endregion
    }
}
