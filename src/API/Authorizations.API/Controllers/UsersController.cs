namespace Authorizations.API.Controllers
{
    [ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region Properties

        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        #endregion

        #region Constructors
        public UsersController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }
        #endregion

        #region CRUD Methods

        /// <summary> Get All Users </summary>
        [HttpGet()]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _userService.GetAllUsersAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


        /// <summary> Get User </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _userService.GetUserByIdAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        /// <summary> Create User </summary>
        /// <param name="userDTO"></param>
        [HttpPost()]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Post([FromBody] UserDTO userDTO)
        {
            try
            {
                userDTO.Id = 0;
                return Ok(await _userService.AddUserAsync(userDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        /// <summary> Login User </summary>
        /// <param name="userLoginDTO"></param>
        [HttpPost("Login")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            try
            {
                UserDTO userDTO = await _userService.GetUserByEmailAsync(userLoginDTO.Email);

                if (!await _userService.LoginUserAsync(userLoginDTO))
                {
                    return Unauthorized(userLoginDTO);
                }

                userDTO.Token = _tokenService.CreateToken(userDTO);

                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        /// <summary> Reset User Password </summary>
        /// <param name="email"></param>
        [HttpPut("ResetPassword/{email}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> ResetPassword([FromRoute] string email)
        {
            try
            {
                return Ok(await _userService.ResetUserPasswordAsync(email));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        /// <summary> Update User Password </summary>
        /// <param name="userLoginDTO"></param>
        [HttpPut("UpdatePassword")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> PutPassword([FromBody] UserLoginDTO userLoginDTO)
        {
            try
            {
                return Ok(await _userService.UpdateUserPasswordAsync(userLoginDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        /// <summary> Update User </summary>
        /// <param name="userDTO"></param>
        /// <param name="id"></param>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UserDTO userDTO)
        {
            try
            {
                userDTO.Id = id;
                return Ok(await _userService.UpdateUserAsync(userDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        /// <summary> Update User Mode </summary>
        /// <param name="mode"></param>
        /// <param name="id"></param>
        [HttpPut("Mode/{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> PutMode([FromRoute] int id, [FromBody] bool mode)
        {
            try
            {
                return Ok(await _userService.UpdateUserModeAsync(id, mode));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        /// <summary> Update User Image </summary>
        /// <param name="image"></param>
        /// <param name="id"></param>
        [HttpPut("Image/{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> PutImage([FromRoute] int id, [FromBody] string image)
        {
            try
            {
                return Ok(await _userService.UpdateUserImageAsync(id, image));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        //// <summary> Delete Users </summary>
        /// <param name="usersIds"></param>
        [HttpPost("Delete")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Delete([FromBody] List<long> usersIds)
        {
            try
            {
                return Ok(await _userService.DeleteUsersAsync(usersIds));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
        #endregion
    }
}
