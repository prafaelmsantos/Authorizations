namespace Authorizations.Core.DTO.Internal.Response
{
    public class InternalBaseResponseDTO
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
