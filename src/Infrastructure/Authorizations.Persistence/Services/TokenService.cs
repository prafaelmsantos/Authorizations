namespace Authorizations.Persistence.Services
{
    public class TokenService : ITokenService
    {
        #region Private variables
        //(chave de segurança- chave de encriptação)
        private readonly string _key;
        #endregion

        #region Constructors
        public TokenService(string key)
        {
            _key = key;
        }
        #endregion

        #region Public methods
        public string CreateToken(UserDTO userDTO)
        {
            List<Claim> claims = GetClaims(userDTO);

            SymmetricSecurityKey tokenKey = new(Encoding.UTF8.GetBytes(_key));

            var creds = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);

            //Montar a estrutura do token.
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                //O token expira em 1 dia
                Expires = DateTime.Now.AddDays(1),
                //chave de criptografia
                SigningCredentials = creds
            };

            //Formato JWT
            var tokenHandler = new JwtSecurityTokenHandler();

            //Criação do token
            var token = tokenHandler.CreateToken(tokenDescription);

            //Escrevo token no formato JWT
            return tokenHandler.WriteToken(token);
        }
        #endregion

        #region Private methods
        private static List<Claim> GetClaims(UserDTO userDTO)
        {
            //Claims são afirmações sobre o utilizador ( nome, idade, foto, ...).
            return new()
            {
                new(ClaimTypes.NameIdentifier, userDTO.Id.ToString()),
                new(ClaimTypes.Name, userDTO.Email)
            };
        }
        #endregion
    }
}
