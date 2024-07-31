namespace Authorizations.API.Controllers
{
    [ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        #region Properties

        private readonly IRoleService _roleService;

        #endregion

        #region Constructors

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        #endregion

        #region CRUD Methods

        /// <summary> Get All Roles </summary>
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _roleService.GetAllRolesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


        /// <summary> Get Role </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                RoleDTO roleDTO = await _roleService.GetRoleByIdAsync(id);
                return Ok(roleDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


        /// <summary> Create Role </summary>
        /// <param name="roleDTO"></param>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Post([FromBody] RoleDTO roleDTO)
        {
            try
            {
                roleDTO.Id = 0;
                return Ok(await _roleService.AddRoleAsync(roleDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


        /// <summary>  Update Mark </summary>
        /// <param name="id"></param>
        /// <param name="roleDTO"></param>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] RoleDTO roleDTO)
        {
            try
            {
                roleDTO.Id = id;
                return Ok(await _roleService.UpdateRoleAsync(roleDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


        //// <summary>  Delete Roles </summary>
        /// <param name="rolesIds"></param>
        [HttpPost("Delete")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Delete([FromBody] List<long> rolesIds)
        {
            try
            {
                return Ok(await _roleService.DeleteRolesAsync(rolesIds));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        #endregion
    }
}
