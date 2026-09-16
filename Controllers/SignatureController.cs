using Microsoft.AspNetCore.Mvc;
using DigitalSignatureSystem.Models;
using DigitalSignatureSystem.Services;

namespace DigitalSignatureSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignatureController : ControllerBase
    {
        private readonly DigitalSignatureService _digitalSignatureService;

        public SignatureController(
            DigitalSignatureService digitalSignatureService)
        {
            _digitalSignatureService = digitalSignatureService;
        }

        // =====================================================
        // 1. GENERATE RSA KEY PAIR
        // =====================================================
        [HttpGet("generate-key")]
        public IActionResult GenerateKey()
        {
            string publicKey =
                _digitalSignatureService.GetPublicKey();

            string privateKey =
                _digitalSignatureService.GetPrivateKey();

            return Ok(new
            {
                message = "RSA key pair generated successfully.",
                publicKey = publicKey,
                privateKey = privateKey
            });
        }


        // =====================================================
        // 2. HASH DATA USING SHA-256
        // =====================================================
        [HttpPost("hash")]
        public IActionResult Hash([FromBody] SignRequest request)
        {
            if (request == null ||
                string.IsNullOrEmpty(request.Data))
            {
                return BadRequest(new
                {
                    message = "Data cannot be empty."
                });
            }

            string hash =
                _digitalSignatureService.ComputeHash(
                    request.Data);

            return Ok(new
            {
                data = request.Data,
                algorithm = "SHA-256",
                hash = hash
            });
        }


        // =====================================================
        // 3. SIGN TEXT DATA
        // =====================================================
        [HttpPost("sign")]
        public IActionResult Sign([FromBody] SignRequest request)
        {
            if (request == null ||
                string.IsNullOrEmpty(request.Data))
            {
                return BadRequest(new
                {
                    message = "Data cannot be empty."
                });
            }

            string hash =
                _digitalSignatureService.ComputeHash(
                    request.Data);

            string signature =
                _digitalSignatureService.SignData(
                    request.Data);

            return Ok(new
            {
                isValid = true,
                message = "Data signed successfully.",
                hash = hash,
                signature = signature
            });
        }


        // =====================================================
        // 4. VERIFY TEXT SIGNATURE
        // =====================================================
        [HttpPost("verify")]
        public IActionResult Verify(
            [FromBody] VerifyRequest request)
        {
            if (request == null ||
                string.IsNullOrEmpty(request.Data) ||
                string.IsNullOrEmpty(request.Signature))
            {
                return BadRequest(new
                {
                    message =
                        "Data and signature are required."
                });
            }

            bool isValid =
                _digitalSignatureService.VerifySignature(
                    request.Data,
                    request.Signature);

            if (isValid)
            {
                string hash =
                    _digitalSignatureService.ComputeHash(
                        request.Data);

                return Ok(new
                {
                    isValid = true,

                    message =
                        "VALID - Signature is correct and data has not been modified.",

                    hash = hash,

                    signature = request.Signature
                });
            }

            return Ok(new
            {
                isValid = false,

                message =
                    "INVALID - Signature is incorrect or data has been modified.",

                hash = (string?)null,

                signature = (string?)null
            });
        }


        // =====================================================
        // 5. SIGN FILE
        // =====================================================
        [HttpPost("sign-file")]
        public IActionResult SignFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    message = "Please upload a file."
                });
            }

            // Đọc file thành byte[]
            using var memoryStream =
                new MemoryStream();

            file.CopyTo(memoryStream);

            byte[] fileData =
                memoryStream.ToArray();


            // Tính SHA-256
            string hash =
                _digitalSignatureService.ComputeFileHash(
                    fileData);


            // Tạo chữ ký RSA
            string signature =
                _digitalSignatureService.SignFile(
                    fileData);


            // =================================================
            // TẠO THƯ MỤC SIGNATURES
            // =================================================

            string signatureDirectory =
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Signatures");

            if (!Directory.Exists(
                    signatureDirectory))
            {
                Directory.CreateDirectory(
                    signatureDirectory);
            }


            // =================================================
            // LƯU CHỮ KÝ THÀNH FILE .SIG
            // =================================================

            string signatureFileName =
                file.FileName + ".sig";

            string signatureFilePath =
                Path.Combine(
                    signatureDirectory,
                    signatureFileName);

            System.IO.File.WriteAllText(
                signatureFilePath,
                signature);


            // =================================================
            // TRẢ KẾT QUẢ
            // =================================================

            return Ok(new
            {
                isValid = true,

                fileName = file.FileName,

                fileSize = file.Length,

                hash = hash,

                signature = signature,

                signatureFile =
                    signatureFileName,

                message =
                    "File signed successfully."
            });
        }


        // =====================================================
        // 6. VERIFY FILE
        // =====================================================
        [HttpPost("verify-file")]
        public IActionResult VerifyFile(
            IFormFile file,
            [FromForm] string signature)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    message =
                        "Please upload a file."
                });
            }

            if (string.IsNullOrEmpty(signature))
            {
                return BadRequest(new
                {
                    message =
                        "Signature is required."
                });
            }


            // Đọc file
            using var memoryStream =
                new MemoryStream();

            file.CopyTo(memoryStream);

            byte[] fileData =
                memoryStream.ToArray();


            // Kiểm tra chữ ký
            bool isValid =
                _digitalSignatureService.VerifyFile(
                    fileData,
                    signature);


            // Tính hash hiện tại của file
            string hash =
                _digitalSignatureService.ComputeFileHash(
                    fileData);


            // =================================================
            // VALID
            // =================================================

            if (isValid)
            {
                return Ok(new
                {
                    isValid = true,

                    fileName = file.FileName,

                    hash = hash,

                    message =
                        "VALID - File signature is correct and file has not been modified.",

                    signature = signature
                });
            }


            // =================================================
            // INVALID
            // =================================================

            return Ok(new
            {
                isValid = false,

                fileName = file.FileName,

                hash = hash,

                message =
                    "INVALID - File has been modified or signature is incorrect.",

                signature = signature
            });
        }
    }
}