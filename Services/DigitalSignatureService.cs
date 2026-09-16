using System.Security.Cryptography;
using System.Text;

namespace DigitalSignatureSystem.Services
{
    public class DigitalSignatureService
    {
        private readonly RSA _rsa;

        public DigitalSignatureService()
        {
            // Tạo cặp khóa RSA 2048 bit
            _rsa = RSA.Create(2048);
        }

        // ==========================================
        // 1. LẤY PUBLIC KEY
        // ==========================================
        public string GetPublicKey()
        {
            return _rsa.ExportRSAPublicKeyPem();
        }

        // ==========================================
        // 2. LẤY PRIVATE KEY
        // ==========================================
        public string GetPrivateKey()
        {
            return _rsa.ExportRSAPrivateKeyPem();
        }

        // ==========================================
        // 3. HASH SHA-256 - DỮ LIỆU TEXT
        // ==========================================
        public string ComputeHash(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            byte[] hash = SHA256.HashData(dataBytes);

            return Convert.ToHexString(hash);
        }

        // ==========================================
        // 4. HASH SHA-256 - FILE
        // ==========================================
        public string ComputeFileHash(byte[] fileData)
        {
            byte[] hash = SHA256.HashData(fileData);

            return Convert.ToHexString(hash);
        }

        // ==========================================
        // 5. KÝ DỮ LIỆU TEXT
        // ==========================================
        public string SignData(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            byte[] signature = _rsa.SignData(
                dataBytes,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1
            );

            return Convert.ToBase64String(signature);
        }

        // ==========================================
        // 6. KÝ FILE
        // ==========================================
        public string SignFile(byte[] fileData)
        {
            byte[] signature = _rsa.SignData(
                fileData,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1
            );

            return Convert.ToBase64String(signature);
        }

        // ==========================================
        // 7. VERIFY DỮ LIỆU TEXT
        // ==========================================
        public bool VerifySignature(
            string data,
            string signatureBase64)
        {
            try
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                byte[] signature =
                    Convert.FromBase64String(signatureBase64);

                return _rsa.VerifyData(
                    dataBytes,
                    signature,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1
                );
            }
            catch
            {
                return false;
            }
        }

        // ==========================================
        // 8. VERIFY FILE
        // ==========================================
        public bool VerifyFile(
            byte[] fileData,
            string signatureBase64)
        {
            try
            {
                byte[] signature =
                    Convert.FromBase64String(signatureBase64);

                return _rsa.VerifyData(
                    fileData,
                    signature,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1
                );
            }
            catch
            {
                return false;
            }
        }
    }
}