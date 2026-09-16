namespace DigitalSignatureSystem.Models
{
    public class SignatureResult
    {
        public bool IsValid { get; set; }

        public string Message { get; set; } = string.Empty;

        public string? Hash { get; set; }

        public string? Signature { get; set; }
    }
}