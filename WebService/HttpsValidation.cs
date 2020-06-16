using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WebService
{
    public class HttpsValidation
    {
        //Call GenerateSSLpubklickey callback method and repalce here   
        static string PUBLIC_KEY = "R E P L A C E - Y O U R P U B L I C K E Y ";
        public static void Initialize()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // ServicePointManager.ServerCertificateValidationCallback = OnValidateCertificate;  
            //Generate Public Key and replace publickey variable   
            //ServicePointManager.ServerCertificateValidationCallback = GenerateSSLPublicKey;  
            ServicePointManager.ServerCertificateValidationCallback = OnValidateCertificate;
        }

        static bool OnValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            var certPublicString = certificate?.GetPublicKeyString();
            PUBLIC_KEY = certPublicString;
            var keysMatch = PUBLIC_KEY == certPublicString;
            return keysMatch;
        }

        static string GenerateSSLPublicKey(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            string certPublicString = certificate?.GetPublicKeyString();
            return certPublicString;
        }
    }
}
