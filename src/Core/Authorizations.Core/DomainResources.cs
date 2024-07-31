namespace Authorizations.Core
{
    /// <summary>
    /// Class Domain Resource for domain exception
    /// </summary>

    public static class DomainResources
    {
        #region User
        public static readonly string UserNotFoundException = "Utilizador não encontrado.";
        public static readonly string UserPasswordNotFoundException = "A palavra-passe do utilizador é invalida.";

        public static readonly string UserIdNeedsToBeSpecifiedException = "O id do utilizador é invalido.";
        public static readonly string DeleteDefaultUserAsyncException = "O utilizador não pode ser apagado pois é padrão do sistema.";
        public static readonly string UserEmailNeedsToBeSpecifiedException = "O email do utilizador é invalido.";
        public static readonly string UserPhoneNumberNeedsToBeSpecifiedException = "O contacto do utilizador é invalido.";
        public static readonly string UserFirstNameNeedsToBeSpecifiedException = "O primeiro nome do utilizador é invalido.";
        public static readonly string UserLastNameNeedsToBeSpecifiedException = "O ultimo nome do utilizador é invalido.";
        public static readonly string UserImageNeedsToBeSpecifiedException = "A imagem do utilizador é invalida.";
        public static readonly string UserPasswordHashNeedsToBeSpecifiedException = "A palavra-passe do utilizador é invalida.";
        public static readonly string UserAlreadyExistsException = "O utilizador já existe.";

        public static readonly string GetAllUsersAsyncException = "Erro ao tentar encontrar utilizadores.";
        public static readonly string GetUserByIdAsyncException = "Erro ao tentar encontrar utilizador por id.";
        public static readonly string GetUserByEmailAsyncException = "Erro ao tentar encontrar utilizador por email.";
        public static readonly string LoginUserAsyncException = "Erro ao tentar iniciar sessão do utilizador.";
        public static readonly string AddUserAsyncException = "Erro ao tentar criar o utilizador.";
        public static readonly string UpdateUserAsyncException = "Erro ao tentar atualizar o utilizador.";
        public static readonly string UpdateUserModeAsyncException = "Erro ao tentar atualizar o modo do utilizador.";
        public static readonly string UpdateUserImageAsyncException = "Erro ao tentar atualizar a foto de perfil do utilizador.";
        public static readonly string UpdateUserPasswordAsyncException = "Erro ao tentar atualizar a palavra-passe do utilizador.";
        public static readonly string ResetUserPasswordAsyncException = "Erro ao tentar criar uma nova palavra-passe do utilizador.";
        public static readonly string DeleteUsersAsyncException = "Erro ao tentar apagar utilizadores.";
        #endregion

        #region Role
        public static readonly string RoleNotFoundException = "Cargo não encontrado.";

        public static readonly string DefaultRoleNotFoundException = "Cargo padrão não encontrado.";
        public static readonly string RoleIdNeedsToBeSpecifiedException = "O id do cargo é invalido.";
        public static readonly string RoleNameNeedsToBeSpecifiedException = "O nome do cargo é invalido.";
        public static readonly string RoleAlreadyExistsException = "O cargo já existe.";

        public static readonly string AddRoleAsyncException = "Erro ao tentar criar o cargo.";
        public static readonly string UpdateRoleAsyncException = "Erro ao tentar atualizar o cargo.";
        public static readonly string GetAllRolesAsyncException = "Erro ao tentar encontrar cargos.";
        public static readonly string GetRoleByIdAsyncException = "Erro ao tentar encontrar cargo por id.";
        public static readonly string UpdateDefaultRoleException = "O cargo não pode ser atualizado pois é padrão do sistema.";
        public static readonly string DeleteRolesAsyncException = "Erro ao tentar apagar cargos.";
        public static readonly string DeleteDefaultRoleAsyncException = "O cargo não pode ser apagado pois é padrão do sistema.";
        #endregion
    }
}
