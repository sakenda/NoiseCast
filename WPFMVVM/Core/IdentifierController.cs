using System;
using System.Security.Cryptography;
using System.Text;

namespace NoiseCast.Core
{
    public static class IdentifierController
    {
        /// <summary>
        /// Generates a Identifier based on a string. The ID can be reproduced with the same string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GenerateSH256Ident(string text)
        {
            SHA256 shaAlgorithm = new SHA256Managed();
            byte[] shaDigest = shaAlgorithm.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            return BitConverter.ToString(shaDigest);
        }
    }
}