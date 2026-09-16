namespace DigitalSignatureSystem.Models
{
    public class VerifyRequest
    {
        public string Data { get; set; } = string.Empty;

        public string Signature { get; set; } = string.Empty;
    }
}