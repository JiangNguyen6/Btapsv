# DIGITAL SIGNATURE SYSTEM

## 1. Project Overview

The Digital Signature System is a web-based application developed for the CMU-CS 376 – Elements of Network Security course.

The system demonstrates the use of digital signatures to provide data authentication and integrity verification. It uses the RSA public-key cryptographic algorithm for digital signatures and SHA-256 for data hashing.

The system supports both text data and document files.

## 2. Main Objectives

The main objectives of the project are:

- Generate an RSA public and private key pair.
- Generate SHA-256 hashes for input data.
- Sign text data using an RSA private key.
- Verify text signatures using an RSA public key.
- Sign document files.
- Verify document file signatures.
- Detect modified data through digital signature verification.
- Display `VALID` or `INVALID` verification results.

## 3. Technologies Used

| Technology | Purpose |
|---|---|
| C# | Main programming language |
| ASP.NET Core Web API | Backend API development |
| System.Security.Cryptography | Cryptographic operations |
| RSA | Digital signature algorithm |
| SHA-256 | Data hashing algorithm |
| HTML | Web interface structure |
| CSS | Web interface styling |
| JavaScript | Frontend interaction and API requests |
| Swagger / OpenAPI | API testing |
| Local File System | Test data and signature storage |

## 4. System Architecture

The system follows a simple web-based architecture:

```text
Web Interface
HTML / CSS / JavaScript
        |
        | HTTP Request
        v
ASP.NET Core Web API
SignatureController
        |
        v
DigitalSignatureService
        |
        +----------------------+
        |                      |
        v                      v
     SHA-256                  RSA
     Hashing              Digital Signature
        |                      |
        +----------+-----------+
                   |
                   v
            Verification
                   |
             VALID / INVALID